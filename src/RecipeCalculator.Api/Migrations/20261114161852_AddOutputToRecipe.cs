using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCalculator.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOutputToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OutputQuantity",
                table: "Recipes",
                type: "TEXT",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutputUnit",
                table: "Recipes",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputQuantity",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "OutputUnit",
                table: "Recipes");
        }
    }
}
