using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCalculator.Api.Data;
using RecipeCalculator.Api.Dtos;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RecipeResponse>>> GetAll()
    {
        var recipes = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.Ingredient)
            .ToListAsync();

        return Ok(recipes.Select(RecipeResponse.From).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeResponse>> GetById(int id)
    {
        var recipe = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.Ingredient)
            .FirstOrDefaultAsync(recipe => recipe.Id == id);

        if (recipe is null)
        {
            return NotFound();
        }

        return Ok(RecipeResponse.From(recipe));
    }

    [HttpPost]
    public async Task<ActionResult<RecipeResponse>> Create(RecipeRequest request)
    {
        var ingredientIds = request
            .RecipeLines.Select(line => line.IngredientId)
            .Distinct()
            .ToList();
        var existingCount = await db.Ingredients.CountAsync(ingredient =>
            ingredientIds.Contains(ingredient.Id)
        );

        if (existingCount != ingredientIds.Count)
        {
            return BadRequest("One or more ingredients do not exist.");
        }

        var recipe = new Recipe
        {
            Name = request.Name,
            Portions = request.Portions,
            RecipeLines = request
                .RecipeLines.Select(line => new RecipeLine
                {
                    IngredientId = line.IngredientId,
                    Quantity = line.Quantity,
                })
                .ToList(),
        };

        db.Add(recipe);
        await db.SaveChangesAsync();

        var created = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.Ingredient)
            .FirstAsync(saved => saved.Id == recipe.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            RecipeResponse.From(created)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var recipe = await db.Recipes.FindAsync(id);

        if (recipe is null)
        {
            return NotFound();
        }

        db.Remove(recipe);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RecipeResponse>> Update(int id, RecipeRequest request)
    {
        var existing = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
            .FirstOrDefaultAsync(recipe => recipe.Id == id);

        if (existing is null)
        {
            return NotFound();
        }

        var ingredientIds = request
            .RecipeLines.Select(line => line.IngredientId)
            .Distinct()
            .ToList();
        var existingCount = await db.Ingredients.CountAsync(ingredient =>
            ingredientIds.Contains(ingredient.Id)
        );

        if (existingCount != ingredientIds.Count)
        {
            return BadRequest("One or more ingredients do not exist.");
        }

        existing.Name = request.Name;
        existing.Portions = request.Portions;

        existing.RecipeLines.Clear();
        foreach (var line in request.RecipeLines)
        {
            existing.RecipeLines.Add(
                new RecipeLine { IngredientId = line.IngredientId, Quantity = line.Quantity }
            );
        }

        await db.SaveChangesAsync();

        var updated = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.Ingredient)
            .FirstAsync(recipe => recipe.Id == id);

        return Ok(RecipeResponse.From(updated));
    }
}
