using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BackFacturas.Models
{
    public class Ciudad
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Departamento { get; set; }
    }
}
