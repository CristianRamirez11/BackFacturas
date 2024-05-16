using BackFacturas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BackFacturas.ConexionDbContext
{
    public class AplicationDbContext: DbContext
    {
        DbSet<Factura> Factura { get; set; }
        DbSet<Articulo> Articulo { get; set; }
        DbSet<Ciudad> Ciudade { get; set; }

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options): base(options)
        {

        }
    }
}
