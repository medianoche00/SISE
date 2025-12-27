using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiseApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAdministrativoYCargo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargoAdministrativo",
                columns: table => new
                {
                    idCargo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreCargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoAdministrativo", x => x.idCargo);
                });

            migrationBuilder.CreateTable(
                name: "Administrativo",
                columns: table => new
                {
                    idAdministrativo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCargoAdministrativo = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    idPersona = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    idCargoAdministrativo1 = table.Column<int>(type: "int", nullable: false),
                    idPersona1 = table.Column<int>(type: "int", nullable: false),
                    idUsuario1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrativo", x => x.idAdministrativo);
                    table.ForeignKey(
                        name: "FK_Administrativo_Cargo",
                        column: x => x.idCargoAdministrativo1,
                        principalTable: "CargoAdministrativo",
                        principalColumn: "idCargo");
                    table.ForeignKey(
                        name: "FK_Administrativo_Persona",
                        column: x => x.idPersona1,
                        principalTable: "Persona",
                        principalColumn: "idPersona");
                    table.ForeignKey(
                        name: "FK_Administrativo_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrativo_idCargoAdministrativo1",
                table: "Administrativo",
                column: "idCargoAdministrativo1");

            migrationBuilder.CreateIndex(
                name: "IX_Administrativo_idPersona1",
                table: "Administrativo",
                column: "idPersona1");

            migrationBuilder.CreateIndex(
                name: "IX_Administrativo_idUsuario",
                table: "Administrativo",
                column: "idUsuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrativo");

            migrationBuilder.DropTable(
                name: "CargoAdministrativo");
        }
    }
}
