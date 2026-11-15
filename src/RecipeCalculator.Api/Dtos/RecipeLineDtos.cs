using System.ComponentModel.DataAnnotations;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Dtos;

public record RecipeLineResponse(
    int Id,
    int? IngredientId,
    int? SubRecipeId,
    string Name,
    decimal Quantity,
    decimal? LineCost
)
{
    public static RecipeLineResponse From(RecipeLine line) =>
    new(
        line.Id,
        line.IngredientId,
        line.SubRecipeId,
        line.Ingredient?.Name ?? line.SubRecipe?.Name ?? "",
        line.Quantity,
        line.LineCost
    );
}

public record RecipeLineRequest(
    [Range(1, int.MaxValue)] int IngredientId,
    [Range(0.0, 1000000.0, MinimumIsExclusive = true)] decimal Quantity
);
