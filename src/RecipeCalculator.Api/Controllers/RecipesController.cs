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
            .Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.SubRecipe)
                    .ThenInclude(subRecipe => subRecipe!.RecipeLines)
                        .ThenInclude(subLine => subLine.Ingredient)
            .ToListAsync();

        return Ok(recipes.Select(RecipeResponse.From).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeResponse>> GetById(int id)
    {
        var recipe = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.Ingredient)
            .Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.SubRecipe)
                    .ThenInclude(subRecipe => subRecipe!.RecipeLines)
                        .ThenInclude(subLine => subLine.Ingredient)
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
            .Where(id => id is not null)
            .Distinct()
            .ToList();
        var existingCount = await db.Ingredients.CountAsync(ingredient =>
            ingredientIds.Contains(ingredient.Id)
        );

        if (existingCount != ingredientIds.Count)
        {
            return Problem(statusCode: 400, title: "One or more ingredients do not exist.");
        }

        var subRecipeIds = request
            .RecipeLines.Select(line => line.SubRecipeId)
            .Where(id => id is not null)
            .Distinct()
            .ToList();
        var existingSubRecipeCount = await db.Recipes.CountAsync(subRecipe =>
            subRecipeIds.Contains(subRecipe.Id) && subRecipe.OutputQuantity != null
        );

        if (existingSubRecipeCount != subRecipeIds.Count)
        {
            return Problem(
                statusCode: 400,
                title: "One or more sub-recipes do not exist or have no output quantity."
            );
        }

        var recipe = new Recipe
        {
            Name = request.Name,
            Portions = request.Portions,
            RecipeLines = request
                .RecipeLines.Select(line => new RecipeLine
                {
                    IngredientId = line.IngredientId,
                    SubRecipeId = line.SubRecipeId,
                    Quantity = line.Quantity,
                })
                .ToList(),
            OutputQuantity = request.OutputQuantity,
            OutputUnit = request.OutputUnit,
        };

        db.Add(recipe);
        await db.SaveChangesAsync();

        var created = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.Ingredient)
            .Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.SubRecipe)
                    .ThenInclude(subRecipe => subRecipe!.RecipeLines)
                        .ThenInclude(subLine => subLine.Ingredient)
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

        var isUsedAsSubRecipe = await db.RecipeLines.AnyAsync(line => line.SubRecipeId == id);

        if (isUsedAsSubRecipe)
        {
            return Problem(
                statusCode: 409,
                title: "Cannot delete a recipe that is used in another recipe."
            );
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
            .Where(id => id is not null)
            .Distinct()
            .ToList();
        var existingIngredientCount = await db.Ingredients.CountAsync(ingredient =>
            ingredientIds.Contains(ingredient.Id)
        );

        if (existingIngredientCount != ingredientIds.Count)
        {
            return Problem(statusCode: 400, title: "One or more ingredients do not exist.");
        }

        var subRecipeIds = request
            .RecipeLines.Select(line => line.SubRecipeId)
            .Where(id => id is not null)
            .Distinct()
            .ToList();

        if (await WouldCreateCycle(id, subRecipeIds.OfType<int>().ToList()))
        {
            return Problem(
                statusCode: 400,
                title: "One of these sub-recipes already uses this recipe, so it can't be added here."
            );
        }

        var existingSubRecipeCount = await db.Recipes.CountAsync(subRecipe =>
            subRecipeIds.Contains(subRecipe.Id) && subRecipe.OutputQuantity != null
        );

        if (existingSubRecipeCount != subRecipeIds.Count)
        {
            return Problem(
                statusCode: 400,
                title: "One or more sub-recipes do not exist or have no output quantity."
            );
        }

        existing.Name = request.Name;
        existing.Portions = request.Portions;
        existing.OutputQuantity = request.OutputQuantity;
        existing.OutputUnit = request.OutputUnit;

        existing.RecipeLines.Clear();
        foreach (var line in request.RecipeLines)
        {
            existing.RecipeLines.Add(
                new RecipeLine
                {
                    IngredientId = line.IngredientId,
                    SubRecipeId = line.SubRecipeId,
                    Quantity = line.Quantity,
                }
            );
        }

        await db.SaveChangesAsync();

        var updated = await db
            .Recipes.Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.Ingredient)
            .Include(recipe => recipe.RecipeLines)
                .ThenInclude(line => line.SubRecipe)
                    .ThenInclude(subRecipe => subRecipe!.RecipeLines)
                        .ThenInclude(subLine => subLine.Ingredient)
            .FirstAsync(recipe => recipe.Id == id);

        return Ok(RecipeResponse.From(updated));
    }

    private async Task<bool> WouldCreateCycle(int recipeId, List<int> subRecipeIds)
    {
        var toCheck = new Queue<int>(subRecipeIds);
        var checkedIds = new HashSet<int>();

        while (toCheck.Count > 0)
        {
            var current = toCheck.Dequeue();

            if (current == recipeId)
            {
                return true;
            }

            if (!checkedIds.Add(current))
            {
                continue;
            }

            var nestedIds = await db
                .RecipeLines.Where(line => line.RecipeId == current && line.SubRecipeId != null)
                .Select(line => line.SubRecipeId!.Value)
                .ToListAsync();

            foreach (var nestedId in nestedIds)
            {
                toCheck.Enqueue(nestedId);
            }
        }

        return false;
    }
}
