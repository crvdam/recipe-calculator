using Microsoft.EntityFrameworkCore;
using RecipeCalculator.Api.Entities;

namespace RecipeCalculator.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeLine> RecipeLines => Set<RecipeLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<RecipeLine>()
            .HasOne(line => line.Ingredient)
            .WithMany()
            .HasForeignKey(line => line.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
