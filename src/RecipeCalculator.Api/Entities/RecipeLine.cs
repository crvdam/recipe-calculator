using Microsoft.EntityFrameworkCore;

namespace RecipeCalculator.Api.Entities;

public class RecipeLine
{
    public int Id { get; set; }
    public int RecipeId { get; set; }

    public Recipe Recipe { get; set; } = null!;

    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;

    [Precision(10, 2)]
    public decimal Quantity { get; set; }

    public decimal? LineCost
    {
        get
        {
            if (Ingredient.YieldPercentage <= 0)
            {
                return null;
            }

            var yieldFraction = Ingredient.YieldPercentage / 100m;
            var grossQuantity = Quantity / yieldFraction;

            return grossQuantity * Ingredient.CostPerBaseUnit;
        }
    }
}
