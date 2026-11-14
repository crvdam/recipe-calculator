using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Tests.Entities;

public class RecipeTests
{
    [Fact]
    public void TotalCost_SumsLineCosts()
    {
        var recipe = CreateRecipe(1, CreateLine(0.25m, 10m), CreateLine(0.10m, 10m));

        Assert.Equal(3.5m, recipe.TotalCost);
    }

    [Fact]
    public void TotalCost_WithLineWithoutPrice_ReturnsNull()
    {
        var recipe = CreateRecipe(1, CreateLine(0.25m, 10m), CreateLine(null, 10m));

        Assert.Null(recipe.TotalCost);
    }

    [Fact]
    public void TotalCost_WithoutLine_ReturnsZero()
    {
        var recipe = CreateRecipe(1);

        Assert.Equal(0m, recipe.TotalCost);
    }

    [Fact]
    public void CostPerPortion_DividesTotalCostByPortions()
    {
        var recipe = CreateRecipe(10, CreateLine(0.25m, 10m), CreateLine(0.10m, 10m));

        Assert.Equal(0.35m, recipe.CostPerPortion);
    }

    private static RecipeLine CreateLine(decimal? pricePerUnit, decimal quantity)
    {
        return new RecipeLine
        {
            Quantity = quantity,
            Ingredient = new Ingredient
            {
                Name = "Test ingredient",
                BaseUnit = Unit.Gram,
                PurchasePrice = pricePerUnit,
                PurchaseQuantity = 1m,
            },
        };
    }

    private static Recipe CreateRecipe(int portions, params RecipeLine[] lines)
    {
        return new Recipe
        {
            Name = "Test recipe",
            RecipeLines = [.. lines],
            Portions = portions,
        };
    }
}