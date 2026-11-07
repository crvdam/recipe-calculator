using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCalculator.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseInfoToIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseUnit",
                table: "Ingredients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Ingredients",
                type: "TEXT",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseQuantity",
                table: "Ingredients",
                type: "TEXT",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YieldPercentage",
                table: "Ingredients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUnit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PurchaseQuantity",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "YieldPercentage",
                table: "Ingredients");
        }
    }
}
