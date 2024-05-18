using BackFacturas.Models;
using System.ComponentModel.DataAnnotations;

namespace BackFacturas.DTOs.DTOSalidas
{
    public class FacturaNuevaRegistradaOutputDTO
    {
        public int NumeroFactura { get; set; }
        public string Fecha { get; set; }
        public string NombreCompletoCliente { get; set; }
        public string NumeroCelular { get; set; }
        public string Email { get; set; }
        public string CiudadCliente { get; set; }
        public string NombreArticulo { get; set; }
        public double ValorTotal { get; set; }

    }
}
