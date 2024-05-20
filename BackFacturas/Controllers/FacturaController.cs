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
                            ValorTotal = detalleFactura.Cantidad * detalleFactura.Articulo.Precio,
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
        public async Task<ActionResult<IEnumerable<ResumenCiudadOutputDTO>>> GetResumenPorCiudad()
        {
            var resumen = await _context.DetalleFacturas
                .Include(df => df.Factura.Cliente.Ciudad)
                .GroupBy(df => df.Factura.Cliente.Ciudad)
                .Select(g => new ResumenCiudadOutputDTO
                {
                    NombreCiudad = g.First().Factura.Cliente.Ciudad.Nombre,
                    TotalValorVendidoCiudad = g.Sum(f => f.ValorTotal)
                })
                .ToListAsync();

            if (resumen == null)
                return NotFound();

            return Ok(resumen);
        }

        // POST api/<FacturaController>
        [HttpPost("RegistrarVentaDetalleFacturaAndFactura")]
        public async Task<ActionResult<FacturaNuevaRegistradaOutputDTO>> PostFactura([FromBody] NuevaFacturaInputDTO nuevoRegistroDetalleFactura)
        {
            try
            {
                Articulo articulo = await _context.Articulos.FirstAsync(a => a.ArticuloId == nuevoRegistroDetalleFactura.ArticuloId);
                if (articulo.Disponibilidad)
                {
                    if (nuevoRegistroDetalleFactura.Cantidad <= articulo.Stock)
                    {
                        Cliente cliente = await _context.Clientes
                                                        .Include(c => c.Ciudad)
                                                        .FirstAsync(c => c.ClienteId == nuevoRegistroDetalleFactura.ClienteId);

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
                            NombreCiudad = cliente.Ciudad.Nombre,
                            NombreArticulo = articulo.Nombre,
                            NumeroFactura = ultimaFacturaRegistrada.NumeroFactura,
                            Factura = ultimaFacturaRegistrada,
                            ArticuloId = articulo.ArticuloId,
                            Articulo = articulo
                        };

                        _context.DetalleFacturas.Add(nuevoDetalleFactura);
                        await _context.SaveChangesAsync();

                        FacturaNuevaRegistradaOutputDTO facturaNuevaRegistradaDTO = new()
                        {
                            NumeroDetalle = ultimaFacturaRegistrada.DetalleFactura.NumeroDetalle,
                            Fecha = ultimaFacturaRegistrada.Fecha,
                            NombreCompletoCliente = cliente.Nombre + " " + cliente.Apellido,
                            NumeroCelular = cliente.Celular,
                            Email = cliente.Email,
                            CiudadCliente = cliente.Ciudad.Nombre + ", " + cliente.Ciudad.Departamento,
                            NombreArticulo = articulo.Nombre,
                            ValorTotal = nuevoRegistroDetalleFactura.Cantidad * articulo.Precio
                        };

                        articulo.Stock -= nuevoRegistroDetalleFactura.Cantidad;
                        if (articulo.Stock == 0)
                            articulo.Disponibilidad = false;

                        await _context.SaveChangesAsync();

                        return Ok(facturaNuevaRegistradaDTO);
                    }
                    else
                        return BadRequest(new { message = "La cantidad seleccionada supera el Stock disponible del producto !!!" });
                }
                else
                {
                    return BadRequest(new { message ="No hay Stock disponible de este producto !!!"});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<FacturaController>/5
        [HttpPut("ActualizarDetalleFactura")]
        public async Task<IActionResult> PutFactura([FromBody] ActualizarDetalleFacturaInputDTO actualizarDetalleFactura)
        {
            try
            {
                if (actualizarDetalleFactura.NumeroDetalle == 0 || actualizarDetalleFactura.NumeroDetalle < 0)
                    return BadRequest();

                Articulo? articuloNuevo = await _context.Articulos.FirstOrDefaultAsync(a => a.ArticuloId == actualizarDetalleFactura.ArticuloId);
                if (articuloNuevo == null)
                {
                    return NotFound(new { message = "Articulo no encontrada" });
                }

                if (articuloNuevo.Disponibilidad && actualizarDetalleFactura.Cantidad <= articuloNuevo.Stock)
                {

                    DetalleFactura? detalleFacturaActualizar = await _context.DetalleFacturas
                                                                             .Include(df => df.Factura)
                                                                             .Include(df => df.Factura.Cliente)
                                                                             .Include(df => df.Factura.Cliente.Ciudad)
                                                                             .FirstOrDefaultAsync(df => df.NumeroDetalle == actualizarDetalleFactura.NumeroDetalle);

                    if (detalleFacturaActualizar == null)
                    {
                        return NotFound(new { message = "Factura no encontrada" });
                    }

                    Articulo? articuloViejo = await _context.Articulos.FirstOrDefaultAsync(a => a.ArticuloId == detalleFacturaActualizar.ArticuloId);
                    articuloViejo.Stock += detalleFacturaActualizar.Cantidad;
                    articuloViejo.Disponibilidad = true;
                    //Factura factura = await _context.Facturas
                    //                                .Include(c => c.Cliente)
                    //                                .Include(c => c.Cliente.Ciudad)
                    //                                .FirstAsync(f => f.NumeroFactura == actualizarDetalleFactura.NumeroFactura);



                    detalleFacturaActualizar.Cantidad = actualizarDetalleFactura.Cantidad;
                    detalleFacturaActualizar.ValorTotal = actualizarDetalleFactura.Cantidad * articuloNuevo.Precio;
                    detalleFacturaActualizar.NombreCiudad = detalleFacturaActualizar.Factura.Cliente.Ciudad.Nombre;
                    detalleFacturaActualizar.NombreArticulo = articuloNuevo.Nombre;

                    articuloNuevo.Stock -= actualizarDetalleFactura.Cantidad;
                    if (articuloNuevo.Stock == 0)
                        articuloNuevo.Disponibilidad = false;

                    _context.Update(detalleFacturaActualizar);               
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "El detalle de la factura fue actualizado !!!" });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!FacturaExists(actualizarDetalleFactura.NumeroDetalle))
                    return NotFound();
                else              
                    return BadRequest(ex.Message);                
            }
        }

        // DELETE api/<FacturaController>/5
        [HttpDelete("EliminarDetalleFacturaAndFactura/{numeroDetalleFactura}")]
        public async Task<IActionResult> DeleteFactura(int numeroDetalleFactura)
        {
            try
            {
                DetalleFactura? detalleFactura = await _context.DetalleFacturas
                                                               .Include(a => a.Articulo)
                                                               .Include(f => f.Factura)
                                                               .FirstOrDefaultAsync(df => df.NumeroDetalle == numeroDetalleFactura);
                if (detalleFactura == null)
                {
                    return NotFound( new {message = "Factura no encontrada"});
                }

                Articulo? articulo = await _context.Articulos.FindAsync(detalleFactura.ArticuloId);
                articulo.Stock += detalleFactura.Cantidad;
                if (articulo.Stock > 0)
                    articulo.Disponibilidad = true;


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
            return _context.DetalleFacturas.Any(df => df.NumeroDetalle == numeroFactura);
        }

    }
}
