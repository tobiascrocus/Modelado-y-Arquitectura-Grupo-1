using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Biodigestor.Server.Models;

namespace Productos.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método se llama en tiempo de ejecución. Use este método para agregar servicios al contenedor.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registrar AppDbContext en el contenedor de DI
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // Agregar servicios MVC
            services.AddControllersWithViews();
        }

        // Este método se llama en tiempo de ejecución. Use este método para configurar la canalización de solicitudes HTTP.
   
    }
}

