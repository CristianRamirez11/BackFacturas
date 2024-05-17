using BackFacturas.Models;
using System.ComponentModel.DataAnnotations;

namespace BackFacturas.DTOs.DTOSalidas
{
    public class FacturaNuevaRegistradaDTO
    {
        public int NumeroFactura { get; set; }

        public string Fecha { get; set; }

        public int ClienteId { get; set; }
        public string NombreCompletoCliente { get; set; }
        public string NumeroCelular { get; set; }
        public string Email { get; set; }
        public string CiudadCliente { get; set; }
    }
}
