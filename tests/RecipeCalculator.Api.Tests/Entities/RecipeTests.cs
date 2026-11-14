using System.Runtime.InteropServices;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Tests.Entities;

public class RecipeTests
{
    [Fact]
    public void TotalCost_SumsLineCosts()
    {
        var recipe = CreateRecipe(CreateLine(0.25m, 10m), CreateLine(0.10m, 10m));

        Assert.Equal(3.5m, recipe.TotalCost);
    }

    [Fact]
    public void TotalCost_WithLineWithoutPrice_ReturnsNull()
    {
        var recipe = CreateRecipe(CreateLine(0.25m, 10m), CreateLine(null, 10m));

        Assert.Null(recipe.TotalCost);
    }

    [Fact]
    public void TotalCost_WithoutLine_ReturnsZero()
    {
        var recipe = CreateRecipe();

        Assert.Equal(0m, recipe.TotalCost);
    }

    [Fact]
    public void CostPerPortion_DividesTotalCostByPortions()
    {
        var recipe = CreateRecipe(CreateLine(0.25m, 10m), CreateLine(0.10m, 10m));
        recipe.Portions = 10;

        Assert.Equal(0.35m, recipe.CostPerPortion);
    }

        [Fact]
    public void CostPerOutputUnit_DividesTotalCostByOutputQuantity()
    {
        var recipe = CreateRecipe(CreateLine(0.25m, 10m), CreateLine(0.10m, 10m));
        recipe.OutputQuantity = 100;
        recipe.OutputUnit = Unit.Gram;

        Assert.Equal(0.035m, recipe.CostPerOutputUnit);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(null)]
    public void CostPerOutputUnit_ZeroOrNullOutputQuantity_ReturnsNull(double? outputQuantity)
    {
        var recipe = CreateRecipe(CreateLine(0.25m, 10m), CreateLine(0.10m, 10m));
        recipe.OutputQuantity = (decimal?)outputQuantity;
        recipe.OutputUnit = Unit.Gram;

        Assert.Null(recipe.CostPerOutputUnit);
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

    private static Recipe CreateRecipe(params RecipeLine[] lines)
    {
        return new Recipe
        {
            Name = "Test recipe",
            RecipeLines = [.. lines],
        };
    }
}