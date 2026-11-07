using System.ComponentModel.DataAnnotations;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Dtos;

public record RecipeResponse(
    int Id,
    string Name,
    decimal Portions,
    List<RecipeLineResponse> RecipeLines
)
{
    public static RecipeResponse From(Recipe recipe) =>
        new(
            recipe.Id,
            recipe.Name,
            recipe.Portions,
            recipe.RecipeLines.Select(RecipeLineResponse.From).ToList()
        );
}

public record RecipeRequest(
    [Required, StringLength(200)] string Name,
    [Range(0, 1000.0, MinimumIsExclusive = true)] decimal Portions,
    List<RecipeLineRequest> RecipeLines
);
