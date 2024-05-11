using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_FK_to_car : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "Car",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Car_username",
                table: "Car",
                column: "username");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_AppUser_username",
                table: "Car",
                column: "username",
                principalTable: "AppUser",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_AppUser_username",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_username",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "username",
                table: "Car");
        }
    }
}
