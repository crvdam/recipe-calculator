using Microsoft.EntityFrameworkCore;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
}