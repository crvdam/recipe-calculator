using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCalculator.Api.Data;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IngredientsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Ingredient>>> GetAll()
    {
        var ingredients = await db.Ingredients.ToListAsync();
        return Ok(ingredients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ingredient>> GetById(int id)
    {
        var ingredient = await db.Ingredients.FindAsync(id);

        if (ingredient is null) 
        {
            return NotFound();
        }

        return Ok(ingredient);
    }

    [HttpPost]
    public async Task<ActionResult<Ingredient>> Create(Ingredient ingredient)
    {
        db.Add(ingredient);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = ingredient.Id }, ingredient);
    }
}