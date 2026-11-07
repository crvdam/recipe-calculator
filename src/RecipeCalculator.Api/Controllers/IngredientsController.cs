using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCalculator.Api.Data;
using RecipeCalculator.Api.Dtos;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IngredientsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<IngredientResponse>>> GetAll()
    {
        var ingredients = await db
            .Ingredients.Select(ingredient => new IngredientResponse(
                ingredient.Id,
                ingredient.Name,
                ingredient.BaseUnit,
                ingredient.PurchasePrice,
                ingredient.PurchaseQuantity,
                ingredient.YieldPercentage,
                ingredient.CostPerBaseUnit
            ))
            .ToListAsync();
        return Ok(ingredients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientResponse>> GetById(int id)
    {
        var ingredient = await db.Ingredients.FindAsync(id);

        if (ingredient is null)
        {
            return NotFound();
        }

        return Ok(IngredientResponse.From(ingredient));
    }

    [HttpPost]
    public async Task<ActionResult<IngredientResponse>> Create(IngredientRequest request)
    {
        var ingredient = new Ingredient
        {
            Name = request.Name,
            BaseUnit = request.BaseUnit!.Value,
            PurchasePrice = request.PurchasePrice,
            PurchaseQuantity = request.PurchaseQuantity,
            YieldPercentage = request.YieldPercentage ?? 100,
        };

        db.Add(ingredient);
        await db.SaveChangesAsync();

        var response = IngredientResponse.From(ingredient);
        return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ingredient = await db.Ingredients.FindAsync(id);

        if (ingredient is null)
        {
            return NotFound();
        }

        var isUsed = await db.RecipeLines.AnyAsync(line => line.IngredientId == id);

        if (isUsed)
        {
            return Conflict("Ingredient is still used in a recipe");
        }

        db.Remove(ingredient);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, IngredientRequest request)
    {
        var existing = await db.Ingredients.FindAsync(id);

        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = request.Name;
        existing.BaseUnit = request.BaseUnit!.Value;
        existing.PurchasePrice = request.PurchasePrice;
        existing.PurchaseQuantity = request.PurchaseQuantity;
        existing.YieldPercentage = request.YieldPercentage ?? 100;

        await db.SaveChangesAsync();
        return Ok(IngredientResponse.From(existing));
    }
}
