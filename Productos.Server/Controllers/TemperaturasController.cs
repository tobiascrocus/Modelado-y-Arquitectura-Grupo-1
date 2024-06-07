using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biodigestor.Server.Models;

namespace Biodigestor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //obtenemos acceso a la Bd desde cualquier parte
    public class TemperaturasController : ControllerBase  {
        private readonly AppDbContext _context;
        public TemperaturasController (AppDbContext dbContext) {
            _context = dbContext;
        }

        [HttpPost]
        [Route("/agregar")]
        // Método para guardar un producto en la bd 
        public Task<IActionResult>CrearTemperatura(Temperatura temperatura)  {
            _context.Temperaturas.Add(temperatura);
            _context.SaveChanges();
            return OkResult();
        }

        private Task<IActionResult> OkResult() {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("/lista")]
        public async Task<ActionResult<IEnumerable<Temperatura>>> ListaTemperaturas()  {
            var temperaturas = await _context.Temperaturas.ToListAsync();
            return Ok(temperaturas);
        }

        [HttpGet]
        [Route("/temperatura")]
        public async Task<IActionResult>VerProducto(int id) {
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Temperatura temperatura = await _context.Temperaturas.FindAsync(id);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (temperatura == null)  {
                return NotFound(" No hay producto regstrado con ese Id");
            }
            return Ok(temperatura);
        }

        [HttpPut]
        [Route("/modificar")]
        public async Task<IActionResult> ActualizarProducto(int id, Temperatura temperatura) {
            var productoExistente = await _context.Temperaturas.FindAsync(id);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("borrar")]
        public async Task<IActionResult> EliminarProducto(int id) {
            var productoBorrado = await _context.Temperaturas.FindAsync(id);
            _context.Temperaturas.Remove(productoBorrado!);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
