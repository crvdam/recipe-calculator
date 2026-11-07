using System.ComponentModel.DataAnnotations;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Dtos;

public record RecipeLineResponse(int Id, int IngredientId, string IngredientName, decimal Quantity)
{
    public static RecipeLineResponse From(RecipeLine line) =>
        new(line.Id, line.IngredientId, line.Ingredient.Name, line.Quantity);
}

public record RecipeLineRequest(
    [Range(1, int.MaxValue)] int IngredientId,
    [Range(0.0, 1000000.0, MinimumIsExclusive = true)] decimal Quantity
);
