using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RatingToStoreAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                table: "Stores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RatingRate",
                table: "Stores",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingCount",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "RatingRate",
                table: "Stores");
        }
    }
}
