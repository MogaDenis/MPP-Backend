using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class changed_user_to_owner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_AppUser_username",
                table: "Car");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_Car_username",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "username",
                table: "Car");

            migrationBuilder.AddColumn<int>(
                name: "ownerId",
                table: "Car",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "Owner");

            migrationBuilder.DropIndex(
                name: "IX_Car_ownerId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "ownerId",
                table: "Car");

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "Car",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Username);
                });

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
    }
}
