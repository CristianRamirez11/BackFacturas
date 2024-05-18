namespace BackFacturas.DTOs
{
    public class ActualizarDetalleFacturaInputDTO
    {
        public int NumeroDetalle { get; set; }
        public int Cantidad { get; set; }
        public int ArticuloId { get; set; }
    }   
}
