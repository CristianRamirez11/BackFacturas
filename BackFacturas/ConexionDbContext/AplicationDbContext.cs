using BackFacturas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BackFacturas.ConexionDbContext
{
    public class AplicationDbContext: DbContext
    {
        public DbSet<Factura> Factura { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Ciudad> Ciudade { get; set; }

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options): base(options)
        {

        }
    }
}
