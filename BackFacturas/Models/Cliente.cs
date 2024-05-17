using System.ComponentModel.DataAnnotations;

namespace BackFacturas.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Celular { get; set; }
        public string Email { get; set; }
        [Required]
        public int CiudadId { get; set; }
        [Required]
        public Ciudad Ciudad { get; set; }

        public IList<Factura> Facturas { get; set; }

        public Cliente()
        {
            Facturas = new List<Factura>();
        }
    }
}
