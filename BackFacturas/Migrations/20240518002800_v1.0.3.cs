using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackFacturas.Migrations
{
    /// <inheritdoc />
    public partial class v103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Facturas_FacturaNumeroFactura",
                table: "DetalleFacturas");

            migrationBuilder.DropIndex(
                name: "IX_DetalleFacturas_FacturaNumeroFactura",
                table: "DetalleFacturas");

            migrationBuilder.DropColumn(
                name: "FacturaNumeroFactura",
                table: "DetalleFacturas");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_NumeroFactura",
                table: "DetalleFacturas",
                column: "NumeroFactura",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Facturas_NumeroFactura",
                table: "DetalleFacturas",
                column: "NumeroFactura",
                principalTable: "Facturas",
                principalColumn: "NumeroFactura",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Facturas_NumeroFactura",
                table: "DetalleFacturas");

            migrationBuilder.DropIndex(
                name: "IX_DetalleFacturas_NumeroFactura",
                table: "DetalleFacturas");

            migrationBuilder.AddColumn<int>(
                name: "FacturaNumeroFactura",
                table: "DetalleFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_FacturaNumeroFactura",
                table: "DetalleFacturas",
                column: "FacturaNumeroFactura");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Facturas_FacturaNumeroFactura",
                table: "DetalleFacturas",
                column: "FacturaNumeroFactura",
                principalTable: "Facturas",
                principalColumn: "NumeroFactura",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
