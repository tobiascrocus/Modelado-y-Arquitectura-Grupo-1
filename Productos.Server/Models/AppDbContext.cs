// AppDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace Biodigestor.Server.Models{
public class AppDbContext : DbContext
{
    public DbSet<Temperatura> Temperaturas { get; set; }
    public DbSet<Volume> Volumes { get; set; }

    // Constructor que acepta DbContextOptions y lo pasa al constructor base de DbContext
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Configuración de modelos y relaciones
        }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  {
        optionsBuilder.UseSqlite("Data Source=biodigestor.db");
    }
}
}