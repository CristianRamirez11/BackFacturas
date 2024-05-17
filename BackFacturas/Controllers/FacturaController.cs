using BackFacturas.ConexionDbContext;
using BackFacturas.DTOs;
using BackFacturas.DTOs.DTOSalidas;
using BackFacturas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackFacturas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public FacturaController(AplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        // GET: api/<FacturaController>
        [HttpGet]
        public async Task<IActionResult> GetAllFacturas()
        {
            try
            {

                var listaFacturas = await _context.Facturas.ToListAsync();

                return Ok(listaFacturas);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<FacturaController>/5
        [HttpGet("{numeroFactura}")]
        public async Task<ActionResult<Factura>> GetFactura(int numeroFactura)
        {
            try
            {
                Factura? facturaEncontrada = await _context.Facturas
                                                      .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);

                if (facturaEncontrada == null)
                    return NotFound();

                return Ok(facturaEncontrada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Factura/DetalleFacturaPorCiudad
        [HttpGet("DetalleFacturaPorCiudad")]
        public async Task<IActionResult> GetResumenFactura()
        {
            try
            {
                var listaFacturasExistentes = await _context.Facturas.ToListAsync();

                //Se valida si existen facturas registradas
                if (listaFacturasExistentes.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    var listaDestalleFacturas = await _context.DetalleFacturas
                        .Include(df => df.Articulo)
                        .Include(df => df.Factura)
                        .Include(df => df.Factura.Cliente)
                        .Include(df => df.Factura.Cliente.Ciudad)
                        .ToListAsync();

                    List<DetalleFactura> listaARetornar = [];
                    foreach (DetalleFactura detalleFactura in listaDestalleFacturas)
                    {
                        DetalleFactura nuevoDetalleFactura = new()
                        {
                            NumeroDetalle = detalleFactura.NumeroDetalle,
                            Cantidad = detalleFactura.Cantidad,
                            ValorTotal = detalleFactura.ValorTotal,
                            NombreCiudad = detalleFactura.Factura.Cliente.Ciudad.Nombre,
                            NombreArticulo = detalleFactura.Articulo.Nombre,
                            NumeroFactura = detalleFactura.Factura.NumeroFactura,
                        };
                        listaARetornar.Add(nuevoDetalleFactura);
                    }

                    return Ok(listaARetornar);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Factura/ResumenVentasCiudad
        [HttpGet("ResumenTotalVentaCiudad")]
        public async Task<ActionResult<IEnumerable<ResumenCiudadDTO>>> GetResumenPorCiudad()
        {
            var resumen = await _context.DetalleFacturas
                .Include(df => df.Factura.Cliente.Ciudad)
                .GroupBy(df => df.Factura.Cliente.Ciudad)
                .Select(g => new ResumenCiudadDTO
                {
                    NombreCiudad = g.First().Factura.Cliente.Ciudad.Nombre,
                    TotalValorVendidoCiudad = g.Sum(f => f.ValorTotal)
                })
                .ToListAsync();

            return Ok(resumen);
        }

        // POST api/<FacturaController>
        [HttpPost]
        public async Task<ActionResult<FacturaNuevaRegistradaDTO>> PostFactura([FromBody] NuevaFacturaDTO factura)
        {
            try
            {
                Cliente cliente = await _context.Clientes.FirstAsync(c => c.ClienteId == factura.ClienteId);
                Ciudad ciudad = await _context.Ciudades.FirstAsync(c => c.CiudadId == cliente.CiudadId);

                Factura nuevaFactura = new()
                {
                    Fecha = factura.Fecha,
                    ClienteId = cliente.ClienteId,
                    Cliente = cliente,
                    DetalleFacturas = []
                };
                
                _context.Facturas.Add(nuevaFactura);
                await _context.SaveChangesAsync();

                var ultimaFacturaRegistrada = await _context.Facturas.OrderBy(f => f.NumeroFactura).LastOrDefaultAsync();

                FacturaNuevaRegistradaDTO facturaNuevaRegistradaDTO = new()
                {
                    NumeroFactura = ultimaFacturaRegistrada.NumeroFactura,
                    Fecha = ultimaFacturaRegistrada.Fecha,
                    ClienteId = ultimaFacturaRegistrada.ClienteId,
                    NombreCompletoCliente = ultimaFacturaRegistrada.Cliente.Nombre + " " + ultimaFacturaRegistrada.Cliente.Apellido,
                    NumeroCelular = ultimaFacturaRegistrada.Cliente.Celular,
                    Email = ultimaFacturaRegistrada.Cliente.Email,
                    CiudadCliente = ultimaFacturaRegistrada.Cliente.Ciudad.Nombre + ", " + ultimaFacturaRegistrada.Cliente.Ciudad.Departamento
                };

                return Ok(facturaNuevaRegistradaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<FacturaController>/5
        [HttpPut("{numeroFactura}")]
        public async Task<IActionResult> PutFactura(int numeroFactura, Factura factura)
        {

            if (numeroFactura != factura.NumeroFactura)
            {
                return BadRequest();
            }

            _context.Entry(factura).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(numeroFactura))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<FacturaController>/5
        [HttpDelete("{numeroFactura}")]
        public async Task<IActionResult> DeleteFactura(int numeroFactura)
        {

            Factura? factura = await _context.Facturas.FindAsync(numeroFactura);
            if (factura == null)
            {
                return NotFound(numeroFactura);
            }

            _context.Facturas.Remove(factura);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool FacturaExists(int numeroFactura)
        {
            return _context.DetalleFacturas.Any(df => df.NumeroFactura == numeroFactura);
        }

    }
}
