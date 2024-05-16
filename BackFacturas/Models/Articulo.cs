using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BackFacturas.Models
{
    public class Articulo
    {
        public int ArticuloId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public int Cantidad { get; set; }

        public IList<Factura> Facturas { get; set; }

        public Articulo()
        {
            Facturas = new List<Factura>();
        }

    }
}
