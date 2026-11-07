using Microsoft.EntityFrameworkCore;

namespace RecipeCalculator.Api.Entities;

public class Recipe
{
    public int Id { get; set; }
    public required string Name { get; set; }

    [Precision(10, 2)]
    public decimal Portions { get; set; } = 1m;

    public List<RecipeLine> RecipeLines { get; set; } = [];
}
