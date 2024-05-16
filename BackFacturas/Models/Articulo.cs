using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BackFacturas.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        
    }
}
