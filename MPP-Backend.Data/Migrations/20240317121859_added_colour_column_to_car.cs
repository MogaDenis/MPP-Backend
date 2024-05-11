using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class added_colour_column_to_car : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "colour",
                table: "Car",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "colour",
                table: "Car");
        }
    }
}
