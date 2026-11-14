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
) : IValidatableObject 
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PurchaseQuantity is null != PurchasePrice is null)
        {
            yield return new ValidationResult(
                "Purchase quantity and purchase price must be provided together.",
                [nameof(PurchaseQuantity), nameof(PurchasePrice)]
            );
        }
    }
}
