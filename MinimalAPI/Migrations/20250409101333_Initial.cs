using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Propiedades",
                columns: table => new
                {
                    IdPropiedad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propiedades", x => x.IdPropiedad);
                });

            migrationBuilder.InsertData(
                table: "Propiedades",
                columns: new[] { "IdPropiedad", "Activa", "Descripcion", "FechaCreacion", "Nombre", "Ubicacion" },
                values: new object[,]
                {
                    { 1, true, "Casa frente al mar con piscina", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Casa de playa", "Playa del Carmen" },
                    { 2, false, "Departamento moderno en el centro de la ciudad", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Departamento en la ciudad", "Ciudad de México" },
                    { 3, true, "Cabaña acogedora en la montaña", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Cabaña en la montaña", "Valle de Bravo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Propiedades");
        }
    }
}
