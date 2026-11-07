using Microsoft.EntityFrameworkCore;

namespace RecipeCalculator.Api.Entities;

public class Ingredient
{
    public int Id { get; set; }
    public required string Name { get; set; }

    [Precision(10, 2)]
    public decimal? PurchasePrice { get; set; }

    [Precision(10, 2)]
    public decimal? PurchaseQuantity { get; set; }

    public int YieldPercentage { get; set; } = 100;

    public required Unit BaseUnit { get; set; }
}
