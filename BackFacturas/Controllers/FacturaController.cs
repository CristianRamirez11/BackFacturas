using BackFacturas.ConexionDbContext;
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
                var listaFacturas = await _context.Factura
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

                foreach (var factura in listaFacturas)
                {
                    Console.WriteLine($"Numero: {factura.NumeroFactura}, Valor: {factura.ValorTotal}, " +
                          $"Articulo: {factura.NombreArticulo}, Ciudad: {factura.NombreCiudad}");
                }
            
                return Ok(listaFacturas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<FacturaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FacturaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FacturaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FacturaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
