using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraMind_V1.Migrations
{
    /// <inheritdoc />
    public partial class FloraMindTestMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Users_UserID",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Users_UserID",
                table: "Plants");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlants_Plants_PlantID",
                table: "UserPlants");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Users_UserID",
                table: "Contents",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Users_UserID",
                table: "Plants",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlants_Plants_PlantID",
                table: "UserPlants",
                column: "PlantID",
                principalTable: "Plants",
                principalColumn: "PlantID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Users_UserID",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Users_UserID",
                table: "Plants");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlants_Plants_PlantID",
                table: "UserPlants");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Users_UserID",
                table: "Contents",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Users_UserID",
                table: "Plants",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlants_Plants_PlantID",
                table: "UserPlants",
                column: "PlantID",
                principalTable: "Plants",
                principalColumn: "PlantID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
