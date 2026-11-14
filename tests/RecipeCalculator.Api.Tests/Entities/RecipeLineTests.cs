using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Tests.Entities;

public class RecipeLineTests
{
    [Theory]
    [InlineData(50, 0.50)]
    [InlineData(100, 0.25)]
    public void LineCost_AppliesYieldToQuantity(int yieldPercentage, double expectedCost)
    {
        var ingredient = new Ingredient
        {
            Name = "Onion",
            BaseUnit = Unit.Gram,
            PurchasePrice = 2.50m,
            PurchaseQuantity = 1000m,
            YieldPercentage = yieldPercentage,
        };

        var recipeLine =  new RecipeLine
        {
            Ingredient = ingredient,
            Quantity = 100m,
        };

        var cost = recipeLine.LineCost;

        Assert.Equal((decimal?)expectedCost, cost);
    }

    [Fact]
    public void LineCost_WithFreeIngredient_ReturnsZero()
    {
        var ingredient = new Ingredient
        {
            Name = "Onion",
            BaseUnit = Unit.Gram,
            PurchasePrice = 0m,
            PurchaseQuantity = 1000m,
            YieldPercentage = 100,
        };

        var recipeLine =  new RecipeLine
        {
            Ingredient = ingredient,
            Quantity = 100m,
        };

        var cost = recipeLine.LineCost;

        Assert.Equal(0m, cost);
    }

        [Fact]
    public void LineCost_WithoutPrice_ReturnsNull()
    {
        var ingredient = new Ingredient
        {
            Name = "Onion",
            BaseUnit = Unit.Gram,
            PurchasePrice = null,
            PurchaseQuantity = 1000m,
            YieldPercentage = 100,
        };

        var recipeLine =  new RecipeLine
        {
            Ingredient = ingredient,
            Quantity = 100m,
        };

        var cost = recipeLine.LineCost;

        Assert.Null(cost);
    }

    [Fact]
    public void LineCost_WithInvalidYield_ReturnsNull()
    {
        var ingredient = new Ingredient
        {
            Name = "Onion",
            BaseUnit = Unit.Gram,
            PurchasePrice = 2.50m,
            PurchaseQuantity = 1000m,
            YieldPercentage = 0,
        };

        var recipeLine =  new RecipeLine
        {
            Ingredient = ingredient,
            Quantity = 100m,
        };

        var cost = recipeLine.LineCost;

        Assert.Null(cost);
    }
}