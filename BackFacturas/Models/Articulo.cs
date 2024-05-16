using Microsoft.Identity.Client;

namespace BackFacturas.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        
    }
}
