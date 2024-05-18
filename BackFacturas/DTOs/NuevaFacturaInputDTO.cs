using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BackFacturas.DTOs
{
    public class NuevaFacturaInputDTO
    {
        public string Fecha { get; set; }
        public int ClienteId { get; set; }
        public int Cantidad { get; set; }
        public int ArticuloId { get; set; }
        public int CiudadId { get; set; }
    }
}
