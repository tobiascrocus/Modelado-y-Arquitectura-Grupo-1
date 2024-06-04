using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productos.Server.Models;

namespace Productos.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //obtenemos acceso a la Bd desde cualquier parte
    public class ProductosController : ControllerBase 
    {
        private readonly AppDbContext _context;
        public ProductosController (AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpPost]
        [Route("/agregar")]
        // Método para guardar un producto en la bd 
        public Task<IActionResult>CrearProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();

            return OkResult();
        }

        private Task<IActionResult> OkResult()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("/lista")]
        public async Task<ActionResult<IEnumerable<Producto>>> ListaProductos()
        {
            var productos = await _context.Productos.ToListAsync();
            return Ok(productos);
        }

        [HttpGet]
        [Route("/producto")]
        public async Task<IActionResult>VerProducto(int id)
        {
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Producto producto = await _context.Productos.FindAsync(id);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (producto == null)
            {
                return NotFound(" No hay producto regstrado con ese Id");
            }

            return Ok(producto);
        }

        [HttpPut]
        [Route("/modificar")]
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(id);
            //productoExistente!.Nombre = producto.Nombre;
            //productoExistente.Descripcion = producto.Descripcion;
            // productoExistente.Precio = producto.Precio;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("borrar")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var productoBorrado = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(productoBorrado!);

            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
