using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BackFacturas.Models
{
    public class Ciudad
    {
        [Key]
        public int CiudadId { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public IList<Cliente> Clientes { get; set; }

        public Ciudad()
        {
            Clientes = new List<Cliente>();
        }
    }
}
