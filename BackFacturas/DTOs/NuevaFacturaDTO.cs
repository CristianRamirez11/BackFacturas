using System.ComponentModel.DataAnnotations;

namespace BackFacturas.DTOs
{
    public class NuevaFacturaDTO
    {
        public string Fecha { get; set; }

        public int ClienteId { get; set; }
    }
}
