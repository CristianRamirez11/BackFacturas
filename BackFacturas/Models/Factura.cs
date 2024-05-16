using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BackFacturas.Models
{
    public class Factura
    {
        public int FacturaId { get; set; }
        [Required]
        public int NumeroFactura { get; set; }
        [Required]
        public double ValorTotal { get; set; }
        [Required]
        public int ArticuloId { get; set; }
        [Required]
        public Articulo Articulo { get; set; }
        [Required]
        public int CiudadId { get; set; }
        [Required]
        public Ciudad Ciudad { get; set; }
               
    }
}
