using BackFacturas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BackFacturas.ConexionDbContext
{
    public class AplicationDbContext: DbContext
    {
        DbSet<Factura> Facturas { get; set; }
        DbSet<Factura> Articulos { get; set; }
        DbSet<Factura> Ciudades { get; set; }

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options): base(options)
        {

        }
    }
}
