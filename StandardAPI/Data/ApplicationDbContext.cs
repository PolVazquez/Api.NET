using ApiCurso.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ApiCurso.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        // Constructor de la clase ApplicationDbContext
        // Se utiliza para inicializar el contexto de la base de datos con las opciones proporcionadas
        // en el archivo de configuración (appsettings.json)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        // Aqui se definen las tablas de la base de datos
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AppUsuario> AppUsuarios { get; set; }
    }
}