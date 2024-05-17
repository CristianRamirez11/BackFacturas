using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

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

        public IList<DetalleFactura> DetalleFacturas { get; set; }

        public Factura()
        {
            DetalleFacturas = new List<DetalleFactura>();
        }

    }
}
