using System.ComponentModel.DataAnnotations;

namespace BackFacturas.Models
{
    public class Factura
    {
        public int Id { get; set; }
        [Required]
        public int NumeroFactura { get; set; }
        [Required]
        public Articulo Articulo { get; set; }
        [Required]
        public Ciudad Ciudad { get; set; }
        [Required]
        public double Valor { get; set; }
    }
}
