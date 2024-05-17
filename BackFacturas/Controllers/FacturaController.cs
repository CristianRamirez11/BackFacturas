using BackFacturas.ConexionDbContext;
using BackFacturas.Models;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Get()
        {
    
            try
            {
                /*
                var listaFacturas = await _context.Facturas
                    .Include(f => f.Articulo)
                    .Include(f => f.Ciudad)
                    .Select(f => new 
                    {
                        f.NumeroFactura,
                        f.ValorTotal,
                        NombreArticulo = f.Articulo.Nombre,
                        NombreCiudad = f.Ciudad.Nombre
                    })
                    .ToListAsync();
            
                return Ok(listaFacturas);
                */
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<FacturaController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            try
            {
                /*
                Factura? facturaEncontrada = await _context.Factura
                                                      .Include(f => f.Articulo)
                                                      .Include(f => f.Ciudad)
                                                      .FirstOrDefaultAsync(f => f.FacturaId == id);

                if (facturaEncontrada == null)
                    return NotFound();

                return Ok(facturaEncontrada);
                */
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                                                           
        }

        // POST api/<FacturaController>
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(Factura factura)
        {
            /*
            _context.Factura.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFactura), new { id = factura.FacturaId }, factura);
            */
            return Ok();
        }

        // PUT api/<FacturaController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, Factura factura)
        {
            /*
            if (id != factura.FacturaId)
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
                if (!FacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
            */
            return Ok();
        }

        // DELETE api/<FacturaController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            /*
            Factura? factura = await _context.Factura.FindAsync(id);
            if (factura == null)
            {
                return NotFound(id);
            }

            _context.Factura.Remove(factura);

            await _context.SaveChangesAsync();

            return NoContent();
            */
            return Ok();
        }

        /*
        private bool FacturaExists(int id)
        {
            return _context.Factura.Any(e => e.FacturaId == id);
        }
        */
    }
}
