namespace BackFacturas.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public int NumeroFactura { get; set; }
        public int NombreArticulo { get; set; }
        public int Ciudad { get; set; }
        public double Valor { get; set; }
    }
}
