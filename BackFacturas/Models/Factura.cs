using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackFacturas.Models
{
    public class Factura
    {
        [Key]
        public int NumeroFactura { get; set; }
        [Required]
        public string Fecha { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public Cliente Cliente { get; set; }
        [Required]
        public DetalleFactura DetalleFactura { get; set; }

    }
}
