using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Biodigestor.Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

/*
            // Crear instancia del contexto
            using (var dbContext = new AppDbContext())
            {
                // Ejemplo: Agregar un nuevo producto a la base de datos
                var nuevoProducto = new Producto { Precio = 10.99m };
                dbContext.Productos.Add(nuevoProducto);
                dbContext.SaveChanges();
                Console.WriteLine("Producto agregado con ID: " + nuevoProducto.Id);
            }
*/

// Configuración del servicio de base de datos (Entity Framework Core)
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", builder =>
    {
        builder.WithOrigins("http://192.168.123.45:5087")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Configuraci�n de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Productos API", Version = "v1" });
});

// Agregar el servicio de Autorizaci�n
builder.Services.AddAuthorization();

// Agregar el servicio de Controladores
builder.Services.AddControllers(); // Agrega el servicio de controladores

var app = builder.Build();

// Configuraci�n del middleware de la aplicaci�n
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Productos API v1"));
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("ReactPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
