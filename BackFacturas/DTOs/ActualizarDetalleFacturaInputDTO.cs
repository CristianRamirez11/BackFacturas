namespace BackFacturas.DTOs
{
    public class ActualizarDetalleFacturaInputDTO
    {
        public int NumeroFactura { get; set; }
        public int NumeroDetalle { get; set; }
        public int Cantidad { get; set; }
        public int ArticuloId { get; set; }
    }   
}
