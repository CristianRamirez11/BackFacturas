using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackFacturas.Models
{
    public class DetalleFactura
    {
        [Key]
        public int NumeroDetalle { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public double ValorTotal { get; set; }
        public string NombreCiudad { get; set; }
        public string NombreArticulo { get; set; }
        [ForeignKey("Factura")]
        public int NumeroFactura { get; set; }
        [Required]
        public Factura Factura { get; set; }
        [Required]
        public int ArticuloId { get; set; }
        [Required]
        public Articulo Articulo { get; set; }

    }
}
