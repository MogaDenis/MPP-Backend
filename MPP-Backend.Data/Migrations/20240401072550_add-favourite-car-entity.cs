using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class addfavouritecarentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouriteCar",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    carId = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteCar", x => x.id);
                    table.ForeignKey(
                        name: "FK_FavouriteCar_Car_carId",
                        column: x => x.carId,
                        principalTable: "Car",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteCar_User_username",
                        column: x => x.username,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteCar_carId",
                table: "FavouriteCar",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteCar_username",
                table: "FavouriteCar",
                column: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteCar");
        }
    }
}
