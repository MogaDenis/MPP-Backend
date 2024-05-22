using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFKOwnerIdToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Car_ownerId",
                table: "Car",
                column: "ownerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Owner_ownerId",
                table: "Car",
                column: "ownerId",
                principalTable: "Owner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Owner_ownerId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_ownerId",
                table: "Car");
        }
    }
}
