using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiseApi.Migrations
{
    /// <inheritdoc />
    public partial class CartaPresentacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cartaPresentacion",
                table: "Postulacion",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cartaPresentacion",
                table: "Postulacion");
        }
    }
}
