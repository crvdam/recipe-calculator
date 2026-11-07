using Microsoft.EntityFrameworkCore;

namespace RecipeCalculator.Api.Entities;

public class Recipe
{
    public int Id { get; set; }
    public required string Name { get; set; }

    [Precision(10, 2)]
    public decimal Portions { get; set; } = 1m;

    public List<RecipeLine> RecipeLines { get; set; } = [];

    public decimal? TotalCost
    {
        get
        {
            if (RecipeLines.Any(line => line.LineCost is null))
            {
                return null;
            }

            return RecipeLines.Sum(line => line.LineCost);
        }
    }

    public decimal? CostPerPortion => Portions is > 0 ? TotalCost / Portions : null;
}
