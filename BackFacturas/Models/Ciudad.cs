using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BackFacturas.Models
{
    public class Ciudad
    {
        public int CiudadId { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public IList<Factura> Facturas { get; set; }

        public Ciudad()
        {
            Facturas = new List<Factura>();
        }
    }
}
