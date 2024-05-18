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
        [HttpPost("RegistrarDetalleFacturaAndFactura")]
        public async Task<ActionResult<FacturaNuevaRegistradaDTO>> PostFactura([FromBody] NuevaFacturaDTO nuevoRegistroDetalleFactura)
        {
            try
            {
                Cliente cliente = await _context.Clientes.FirstAsync(c => c.ClienteId == nuevoRegistroDetalleFactura.ClienteId);
                Articulo articulo = await _context.Articulos.FirstAsync(a => a.ArticuloId == nuevoRegistroDetalleFactura.ArticuloId);
                Ciudad ciudad = await _context.Ciudades.FirstAsync(c => c.CiudadId == nuevoRegistroDetalleFactura.CiudadId);

                Factura nuevaFactura = new()
                {
                    Fecha = nuevoRegistroDetalleFactura.Fecha,
                    ClienteId = cliente.ClienteId,
                    Cliente = cliente
                };
                
                _context.Facturas.Add(nuevaFactura);
                await _context.SaveChangesAsync();

                var ultimaFacturaRegistrada = await _context.Facturas.OrderBy(f => f.NumeroFactura).LastOrDefaultAsync();

                DetalleFactura nuevoDetalleFactura = new()
                {
                    Cantidad = nuevoRegistroDetalleFactura.Cantidad,
                    ValorTotal = nuevoRegistroDetalleFactura.Cantidad * articulo.Precio,
                    NombreCiudad = ciudad.Nombre,
                    NombreArticulo = articulo.Nombre,
                    NumeroFactura = ultimaFacturaRegistrada.NumeroFactura,
                    Factura = ultimaFacturaRegistrada,
                    ArticuloId = articulo.ArticuloId,
                    Articulo = articulo
                };

                _context.DetalleFacturas.Add(nuevoDetalleFactura);
                await _context.SaveChangesAsync();

                FacturaNuevaRegistradaDTO facturaNuevaRegistradaDTO = new()
                {
                    NumeroFactura = ultimaFacturaRegistrada.NumeroFactura,
                    Fecha = ultimaFacturaRegistrada.Fecha,
                    NombreCompletoCliente = ultimaFacturaRegistrada.Cliente.Nombre + " " + ultimaFacturaRegistrada.Cliente.Apellido,
                    NumeroCelular = ultimaFacturaRegistrada.Cliente.Celular,
                    Email = ultimaFacturaRegistrada.Cliente.Email,
                    CiudadCliente = ciudad.Nombre + ", " + ciudad.Departamento,
                    NombreArticulo = articulo.Nombre,
                    ValorTotal = nuevoRegistroDetalleFactura.Cantidad * articulo.Precio
                };

                return Ok(facturaNuevaRegistradaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<FacturaController>/5
        [HttpPut("ActualizarDetalleFactura")]
        public async Task<IActionResult> PutFactura([FromBody] ActualizarDetalleFacturaDTO actualizarDetalleFactura)
        {
            try
            {
                if (actualizarDetalleFactura.NumeroFactura == 0 || actualizarDetalleFactura.NumeroFactura < 0)
                    return BadRequest();

                DetalleFactura? detalleFacturaActualizar = await _context.DetalleFacturas
                                                                        .Include(df => df.Factura)
                                                                        .Include(df => df.Articulo)
                                                                        .FirstOrDefaultAsync(df => df.NumeroDetalle == actualizarDetalleFactura.NumeroDetalle);

                detalleFacturaActualizar.Cantidad = actualizarDetalleFactura.Cantidad;
                detalleFacturaActualizar.ValorTotal = actualizarDetalleFactura.ValorTotal;
                detalleFacturaActualizar.NombreCiudad = actualizarDetalleFactura.NombreCiudad;
                detalleFacturaActualizar.NombreArticulo = actualizarDetalleFactura.NombreArticulo;

                _context.Update(detalleFacturaActualizar);
                await _context.SaveChangesAsync();

                return Ok(new { message = "El detalle de la factura fue actualizado !!!"});
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!FacturaExists(actualizarDetalleFactura.NumeroFactura))
                    return NotFound();
                else              
                    return BadRequest(ex.Message);                
            }
        }

        // DELETE api/<FacturaController>/5
        [HttpDelete("EliminarDetalleFacturaAndFactura{numeroDetalleFactura}")]
        public async Task<IActionResult> DeleteFactura(int numeroDetalleFactura)
        {
            try
            {
                DetalleFactura? detalleFactura = await _context.DetalleFacturas.FindAsync(numeroDetalleFactura);
                if (detalleFactura == null)
                {
                    return NotFound( new {message = "Factura no encontrada"});
                }

                _context.DetalleFacturas.Remove(detalleFactura);
                _context.Facturas.Remove(detalleFactura.Factura);

                await _context.SaveChangesAsync();

                return Ok(new { message = "La Factura y el detalle de la factura fuerón eliminados con exito !!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); ;
            }
        }


        private bool FacturaExists(int numeroFactura)
        {
            return _context.DetalleFacturas.Any(df => df.NumeroFactura == numeroFactura);
        }

    }
}
