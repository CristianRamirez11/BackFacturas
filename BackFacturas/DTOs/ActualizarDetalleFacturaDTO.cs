namespace BackFacturas.DTOs
{
    public class ActualizarDetalleFacturaDTO
    {
        public int NumeroFactura { get; set; }
        public int NumeroDetalle { get; set; }
        public int Cantidad { get; set; }
        public double ValorTotal { get; set; }
        public string NombreCiudad { get; set; }
        public string NombreArticulo { get; set; }
    }   
}
