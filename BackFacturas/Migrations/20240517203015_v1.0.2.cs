using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackFacturas.Migrations
{
    /// <inheritdoc />
    public partial class v102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreArticulo",
                table: "DetalleFacturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreCiudad",
                table: "DetalleFacturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreArticulo",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "NombreCiudad",
                table: "DetalleFacturas");
        }
    }
}
