using System.ComponentModel.DataAnnotations;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Dtos;

public record IngredientResponse(int Id, string Name)
{
    public static IngredientResponse From(Ingredient ingredient) => new(ingredient.Id, ingredient.Name);
}
    
public record IngredientRequest(
    [Required, StringLength(200)] string Name);