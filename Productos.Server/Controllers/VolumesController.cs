using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biodigestor.Server.Models;

namespace Biodigestor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //obtenemos acceso a la Bd desde cualquier parte
    public class VolumesController : ControllerBase  {
        private readonly AppDbContext _context;
        public VolumesController (AppDbContext dbContext) {
            _context = dbContext;
        }

        [HttpPost]
        [Route("/agregar1")]
        // Método para guardar un producto en la bd 
        public Task<IActionResult>CrearVolume(Volume volume)  {
            _context.Volumes.Add(volume);
            _context.SaveChanges();
            return OkResult();
        }

        private Task<IActionResult> OkResult() {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("/lista1")]
        public async Task<ActionResult<IEnumerable<Volume>>> ListaVolumes()  {
            var volumes = await _context.Temperaturas.ToListAsync();
            return Ok(volumes);
        }

        [HttpGet]
        [Route("/volume")]
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
        [Route("/modificar1")]
        public async Task<IActionResult> ActualizarProducto(int id, Temperatura temperatura) {
            var productoExistente = await _context.Temperaturas.FindAsync(id);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("/borrar1")]
        public async Task<IActionResult> EliminarProducto(int id) {
            var productoBorrado = await _context.Temperaturas.FindAsync(id);
            _context.Temperaturas.Remove(productoBorrado!);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
