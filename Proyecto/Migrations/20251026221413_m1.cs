using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrador",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrador", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    IdPelicula = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Duracion = table.Column<int>(type: "integer", nullable: false),
                    Genero = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Estado = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RestriccionEdad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sipnosis = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FechaEstreno = table.Column<DateOnly>(type: "date", nullable: false),
                    ImagenVertical = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImagenHorizontal = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Trailer = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.IdPelicula);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    IdPersona = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumeroDeDocumento = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    FechaDeNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    Genero = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.IdPersona);
                });

            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    Id_sala = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Capacidad = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.Id_sala);
                });

            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    idcuenta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Contrasena = table.Column<string>(type: "text", nullable: false),
                    Cliente = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Administrativo = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Soporte = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Ventas = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Funciones = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.idcuenta);
                    table.ForeignKey(
                        name: "FK_Cuentas_Personas_IdPersona",
                        column: x => x.IdPersona,
                        principalTable: "Personas",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asientos",
                columns: table => new
                {
                    Id_asiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_sala = table.Column<int>(type: "integer", nullable: false),
                    Fila = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Columna = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    SalaId_sala = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asientos", x => x.Id_asiento);
                    table.ForeignKey(
                        name: "FK_Asientos_Salas_SalaId_sala",
                        column: x => x.SalaId_sala,
                        principalTable: "Salas",
                        principalColumn: "Id_sala");
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    idCompra = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idCuenta = table.Column<int>(type: "integer", nullable: false),
                    idPelicula = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.idCompra);
                    table.ForeignKey(
                        name: "FK_Compras_Cuentas_idCuenta",
                        column: x => x.idCuenta,
                        principalTable: "Cuentas",
                        principalColumn: "idcuenta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compras_Peliculas_idPelicula",
                        column: x => x.idPelicula,
                        principalTable: "Peliculas",
                        principalColumn: "IdPelicula",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Funcion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idSala = table.Column<int>(type: "integer", nullable: false),
                    idPelicula = table.Column<int>(type: "integer", nullable: false),
                    Precio = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    horaInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    horaFinal = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    AsientoId_asiento = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcion", x => x.id);
                    table.ForeignKey(
                        name: "FK_Funcion_Asientos_AsientoId_asiento",
                        column: x => x.AsientoId_asiento,
                        principalTable: "Asientos",
                        principalColumn: "Id_asiento");
                    table.ForeignKey(
                        name: "FK_Funcion_Peliculas_idPelicula",
                        column: x => x.idPelicula,
                        principalTable: "Peliculas",
                        principalColumn: "IdPelicula",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funcion_Salas_idSala",
                        column: x => x.idSala,
                        principalTable: "Salas",
                        principalColumn: "Id_sala",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asientos_SalaId_sala",
                table: "Asientos",
                column: "SalaId_sala");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_idCuenta",
                table: "Compras",
                column: "idCuenta");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_idPelicula",
                table: "Compras",
                column: "idPelicula");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_IdPersona",
                table: "Cuentas",
                column: "IdPersona",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_AsientoId_asiento",
                table: "Funcion",
                column: "AsientoId_asiento");

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_idPelicula",
                table: "Funcion",
                column: "idPelicula");

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_idSala",
                table: "Funcion",
                column: "idSala");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrador");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Funcion");

            migrationBuilder.DropTable(
                name: "Cuentas");

            migrationBuilder.DropTable(
                name: "Asientos");

            migrationBuilder.DropTable(
                name: "Peliculas");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Salas");
        }
    }
}
