using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiseApi.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPostulaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postulacion",
                columns: table => new
                {
                    idPostulacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEgresado = table.Column<int>(type: "int", nullable: false),
                    idOferta = table.Column<int>(type: "int", nullable: false),
                    idRepresentanteEvaluador = table.Column<int>(type: "int", nullable: true),
                    fechaPostulacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    fechaEvaluacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pendiente"),
                    comentarios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postulacion", x => x.idPostulacion);
                    table.ForeignKey(
                        name: "FK_Postulacion_Egresado",
                        column: x => x.idEgresado,
                        principalTable: "Egresado",
                        principalColumn: "idEgresado");
                    table.ForeignKey(
                        name: "FK_Postulacion_Oferta",
                        column: x => x.idOferta,
                        principalTable: "OfertaLaboral",
                        principalColumn: "idOferta");
                    table.ForeignKey(
                        name: "FK_Postulacion_Representante",
                        column: x => x.idRepresentanteEvaluador,
                        principalTable: "Representante",
                        principalColumn: "idRepresentante");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Postulacion_idEgresado_idOferta",
                table: "Postulacion",
                columns: new[] { "idEgresado", "idOferta" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Postulacion_idOferta",
                table: "Postulacion",
                column: "idOferta");

            migrationBuilder.CreateIndex(
                name: "IX_Postulacion_idRepresentanteEvaluador",
                table: "Postulacion",
                column: "idRepresentanteEvaluador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Postulacion");
        }
    }
}
