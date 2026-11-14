using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Tests.Entities;

public class IngredientTests
{
    [Fact]
    public void CostPerBaseUnit_WithPriceAndQuantity_ReturnsPricePerUnit()
    {
        var ingredient = new Ingredient
        {
            Name = "Onion",
            BaseUnit = Unit.Gram,
            PurchasePrice = 2.50m,
            PurchaseQuantity = 1000m,
        };

        var cost = ingredient.CostPerBaseUnit;

        Assert.Equal(0.0025m, cost);
    }

    [Theory]
    [InlineData(null, 1000.0)] // no price
    [InlineData(2.50, 0.0)]    // zero quantity
    [InlineData(2.50, -5.0)]   // negative quantity
    [InlineData(2.50, null)]   // no quantity
    public void CostPerBaseUnit_WithMissingOrInvalidInput_ReturnsNull(
        double? price,
        double? quantity
    )
    {
        var ingredient = new Ingredient
        {
            Name = "Onion",
            BaseUnit = Unit.Gram,
            PurchasePrice = (decimal?)price,
            PurchaseQuantity = (decimal?)quantity,
        };

        Assert.Null(ingredient.CostPerBaseUnit);
    }
}
