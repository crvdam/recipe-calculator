using System.ComponentModel.DataAnnotations;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Dtos;

public record IngredientResponse(
    int Id,
    string Name,
    Unit BaseUnit,
    decimal? PurchasePrice,
    decimal? PurchaseQuantity,
    int YieldPercentage,
    decimal? CostPerBaseUnit
)
{
    public static IngredientResponse From(Ingredient ingredient) =>
        new(
            ingredient.Id,
            ingredient.Name,
            ingredient.BaseUnit,
            ingredient.PurchasePrice,
            ingredient.PurchaseQuantity,
            ingredient.YieldPercentage,
            ingredient.CostPerBaseUnit
        );
}

public record IngredientRequest(
    [Required, StringLength(200)] string Name,
    [Required] Unit? BaseUnit,
    [Range(0, 1000000.0, MinimumIsExclusive = true)] decimal? PurchasePrice,
    [Range(0, 1000000.0, MinimumIsExclusive = true)] decimal? PurchaseQuantity,
    [Range(1, 100)] int? YieldPercentage
);
