using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

namespace MinimalAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Propiedad> Propiedades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Propiedad>().HasData(
            [
                new()
                {
                    IdPropiedad = 1,
                    Nombre = "Casa de playa",
                    Descripcion = "Casa frente al mar con piscina",
                    Ubicacion = "Playa del Carmen",
                    Activa = true,
                    FechaCreacion = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc)
                },
                new()
                {
                    IdPropiedad = 2,
                    Nombre = "Departamento en la ciudad",
                    Descripcion = "Departamento moderno en el centro de la ciudad",
                    Ubicacion = "Ciudad de México",
                    Activa = false,
                    FechaCreacion = new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)
                },
                new()
                {
                    IdPropiedad = 3,
                    Nombre = "Cabaña en la montaña",
                    Descripcion = "Cabaña acogedora en la montaña",
                    Ubicacion = "Valle de Bravo",
                    Activa = true,
                    FechaCreacion = new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc)
                }
            ]);


            base.OnModelCreating(modelBuilder);
        }
    }
}
