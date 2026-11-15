using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCalculator.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubRecipeToRecipeLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "RecipeLines",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "SubRecipeId",
                table: "RecipeLines",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeLines_SubRecipeId",
                table: "RecipeLines",
                column: "SubRecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeLines_Recipes_SubRecipeId",
                table: "RecipeLines",
                column: "SubRecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeLines_Recipes_SubRecipeId",
                table: "RecipeLines");

            migrationBuilder.DropIndex(
                name: "IX_RecipeLines_SubRecipeId",
                table: "RecipeLines");

            migrationBuilder.DropColumn(
                name: "SubRecipeId",
                table: "RecipeLines");

            migrationBuilder.AlterColumn<int>(
                name: "IngredientId",
                table: "RecipeLines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
