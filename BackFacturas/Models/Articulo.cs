using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BackFacturas.Models
{
    public class Articulo
    {
        [Key]
        public int ArticuloId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public int Stock { get; set; }
        public bool Disponibilidad { get; set; }

        public IList<DetalleFactura> DetalleFacturas { get; set; }

        public Articulo()
        {
            DetalleFacturas = new List<DetalleFactura>();
        }

    }
}
