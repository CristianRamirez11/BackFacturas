using BackFacturas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BackFacturas.ConexionDbContext
{
    public class AplicationDbContext: DbContext
    {
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options): base(options)
        {

        }
    }
}
