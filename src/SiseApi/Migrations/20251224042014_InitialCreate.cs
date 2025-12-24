using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiseApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    idEmpresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ruc = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false),
                    razonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.idEmpresa);
                });

            migrationBuilder.CreateTable(
                name: "Facultad",
                columns: table => new
                {
                    idFacultad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreFacultad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Facultad__B57E5B202D9641D0", x => x.idFacultad);
                });

            migrationBuilder.CreateTable(
                name: "ModalidadTrabajo",
                columns: table => new
                {
                    idModalidadTrabajo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreModalidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Modalida__82081B5D073BB510", x => x.idModalidadTrabajo);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    idPersona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellidoPaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    apellidoMaterno = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    documentoIdentidad = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    correoPersonal = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.idPersona);
                });

            migrationBuilder.CreateTable(
                name: "TipoContrato",
                columns: table => new
                {
                    idTipoContrato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreTipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoContrato", x => x.idTipoContrato);
                });

            migrationBuilder.CreateTable(
                name: "TipoFormacion",
                columns: table => new
                {
                    idTipoFormacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreTipoFormacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoFormacion", x => x.idTipoFormacion);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    idAuditoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUsuario = table.Column<int>(type: "int", nullable: true),
                    tablaAfectada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    columnaAfectada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    accion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    valorAnterior = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valorNuevo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fechaHora = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.idAuditoria);
                    table.ForeignKey(
                        name: "FK_Auditoria_Usuario_Identity",
                        column: x => x.idUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Escuela",
                columns: table => new
                {
                    idEscuela = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idFacultad = table.Column<int>(type: "int", nullable: false),
                    nombreEscuela = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Escuela__9F67B289F35514A8", x => x.idEscuela);
                    table.ForeignKey(
                        name: "FK__Escuela__idFacul__06CD04F7",
                        column: x => x.idFacultad,
                        principalTable: "Facultad",
                        principalColumn: "idFacultad");
                });

            migrationBuilder.CreateTable(
                name: "Representante",
                columns: table => new
                {
                    idRepresentante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEmpresa = table.Column<int>(type: "int", nullable: false),
                    idPersona = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Representante", x => x.idRepresentante);
                    table.ForeignKey(
                        name: "FK_Representante_Empresa",
                        column: x => x.idEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "idEmpresa");
                    table.ForeignKey(
                        name: "FK_Representante_Persona",
                        column: x => x.idPersona,
                        principalTable: "Persona",
                        principalColumn: "idPersona");
                    table.ForeignKey(
                        name: "FK_Representante_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carrera",
                columns: table => new
                {
                    idCarrera = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEscuela = table.Column<int>(type: "int", nullable: false),
                    nombreCarrera = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrera", x => x.idCarrera);
                    table.ForeignKey(
                        name: "FK_Carrera_Escuela",
                        column: x => x.idEscuela,
                        principalTable: "Escuela",
                        principalColumn: "idEscuela");
                });

            migrationBuilder.CreateTable(
                name: "Egresado",
                columns: table => new
                {
                    idEgresado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idPersona = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    idCarrera = table.Column<int>(type: "int", nullable: false),
                    codigoUniversitario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    añoEgreso = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egresado", x => x.idEgresado);
                    table.ForeignKey(
                        name: "FK_Egresado_Carrera",
                        column: x => x.idCarrera,
                        principalTable: "Carrera",
                        principalColumn: "idCarrera");
                    table.ForeignKey(
                        name: "FK_Egresado_Persona",
                        column: x => x.idPersona,
                        principalTable: "Persona",
                        principalColumn: "idPersona");
                    table.ForeignKey(
                        name: "FK_Egresado_Usuario",
                        column: x => x.idUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExperienciaLaboral",
                columns: table => new
                {
                    idExperiencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEgresado = table.Column<int>(type: "int", nullable: false),
                    empresa = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    idEmpresaRegistrada = table.Column<int>(type: "int", nullable: true),
                    cargo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    fechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Experien__77DCF2941080D70A", x => x.idExperiencia);
                    table.ForeignKey(
                        name: "FK_ExperienciaLaboral_Empresa_idEmpresaRegistrada",
                        column: x => x.idEmpresaRegistrada,
                        principalTable: "Empresa",
                        principalColumn: "idEmpresa");
                    table.ForeignKey(
                        name: "FK__Experienc__idEgr__07C12930",
                        column: x => x.idEgresado,
                        principalTable: "Egresado",
                        principalColumn: "idEgresado");
                });

            migrationBuilder.CreateTable(
                name: "FormacionComplementaria",
                columns: table => new
                {
                    idFormacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEgresado = table.Column<int>(type: "int", nullable: false),
                    idTipoFormacion = table.Column<int>(type: "int", nullable: false),
                    nombreDelCurso = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    institucion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    fechaInicio = table.Column<DateOnly>(type: "date", nullable: true),
                    fechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Formacio__9DE85F3DDE9E96E9", x => x.idFormacion);
                    table.ForeignKey(
                        name: "FK__Formacion__idEgr__08B54D69",
                        column: x => x.idEgresado,
                        principalTable: "Egresado",
                        principalColumn: "idEgresado");
                    table.ForeignKey(
                        name: "FK__Formacion__idTip__09A971A2",
                        column: x => x.idTipoFormacion,
                        principalTable: "TipoFormacion",
                        principalColumn: "idTipoFormacion");
                });

            migrationBuilder.CreateTable(
                name: "OfertaLaboral",
                columns: table => new
                {
                    idOferta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEmpresa = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    requisitos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ubicacion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    idTipoContrato = table.Column<int>(type: "int", nullable: false),
                    sueldo = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    idModalidadTrabajo = table.Column<int>(type: "int", nullable: false),
                    fechaPublicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    fechaCierre = table.Column<DateOnly>(type: "date", nullable: false),
                    idEgresadoGanador = table.Column<int>(type: "int", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OfertaLa__05A1245E53519920", x => x.idOferta);
                    table.ForeignKey(
                        name: "FK__OfertaLab__idEgr__0A9D95DB",
                        column: x => x.idEgresadoGanador,
                        principalTable: "Egresado",
                        principalColumn: "idEgresado");
                    table.ForeignKey(
                        name: "FK__OfertaLab__idEmp__0B91BA14",
                        column: x => x.idEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "idEmpresa");
                    table.ForeignKey(
                        name: "FK__OfertaLab__idMod__0C85DE4D",
                        column: x => x.idModalidadTrabajo,
                        principalTable: "ModalidadTrabajo",
                        principalColumn: "idModalidadTrabajo");
                    table.ForeignKey(
                        name: "FK__OfertaLab__idTip__0D7A0286",
                        column: x => x.idTipoContrato,
                        principalTable: "TipoContrato",
                        principalColumn: "idTipoContrato");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_idUsuario",
                table: "Auditoria",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Carrera_idEscuela",
                table: "Carrera",
                column: "idEscuela");

            migrationBuilder.CreateIndex(
                name: "IX_Egresado_codigoUniversitario",
                table: "Egresado",
                column: "codigoUniversitario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Egresado_idCarrera",
                table: "Egresado",
                column: "idCarrera");

            migrationBuilder.CreateIndex(
                name: "IX_Egresado_idPersona",
                table: "Egresado",
                column: "idPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Egresado_idUsuario",
                table: "Egresado",
                column: "idUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_ruc",
                table: "Empresa",
                column: "ruc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Escuela_idFacultad",
                table: "Escuela",
                column: "idFacultad");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciaLaboral_idEgresado",
                table: "ExperienciaLaboral",
                column: "idEgresado");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciaLaboral_idEmpresaRegistrada",
                table: "ExperienciaLaboral",
                column: "idEmpresaRegistrada");

            migrationBuilder.CreateIndex(
                name: "IX_FormacionComplementaria_idEgresado",
                table: "FormacionComplementaria",
                column: "idEgresado");

            migrationBuilder.CreateIndex(
                name: "IX_FormacionComplementaria_idTipoFormacion",
                table: "FormacionComplementaria",
                column: "idTipoFormacion");

            migrationBuilder.CreateIndex(
                name: "IX_ModalidadTrabajo_nombreModalidad",
                table: "ModalidadTrabajo",
                column: "nombreModalidad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfertaLaboral_idEgresadoGanador",
                table: "OfertaLaboral",
                column: "idEgresadoGanador");

            migrationBuilder.CreateIndex(
                name: "IX_OfertaLaboral_idEmpresa",
                table: "OfertaLaboral",
                column: "idEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_OfertaLaboral_idModalidadTrabajo",
                table: "OfertaLaboral",
                column: "idModalidadTrabajo");

            migrationBuilder.CreateIndex(
                name: "IX_OfertaLaboral_idTipoContrato",
                table: "OfertaLaboral",
                column: "idTipoContrato");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_documentoIdentidad",
                table: "Persona",
                column: "documentoIdentidad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Representante_idEmpresa",
                table: "Representante",
                column: "idEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Representante_idPersona",
                table: "Representante",
                column: "idPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Representante_idUsuario",
                table: "Representante",
                column: "idUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoContrato_nombreTipo",
                table: "TipoContrato",
                column: "nombreTipo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "ExperienciaLaboral");

            migrationBuilder.DropTable(
                name: "FormacionComplementaria");

            migrationBuilder.DropTable(
                name: "OfertaLaboral");

            migrationBuilder.DropTable(
                name: "Representante");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TipoFormacion");

            migrationBuilder.DropTable(
                name: "Egresado");

            migrationBuilder.DropTable(
                name: "ModalidadTrabajo");

            migrationBuilder.DropTable(
                name: "TipoContrato");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Carrera");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Escuela");

            migrationBuilder.DropTable(
                name: "Facultad");
        }
    }
}
