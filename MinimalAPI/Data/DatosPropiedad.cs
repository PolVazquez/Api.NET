using MinimalAPI.Models;

namespace MinimalAPI.Data
{
    public static class DatosPropiedad
    {
        public static List<Propiedad> Propiedades { get; set; } =
        [
            new() {
                IdPropiedad = 1,
                Nombre = "Casa de playa",
                Descripcion = "Casa frente al mar con piscina",
                Ubicacion = "Playa del Carmen",
                Activa = true,
                FechaCreacion = DateTime.UtcNow
            },
            new() {
                IdPropiedad = 2,
                Nombre = "Departamento en la ciudad",
                Descripcion = "Departamento moderno en el centro de la ciudad",
                Ubicacion = "Ciudad de México",
                Activa = false,
                FechaCreacion = DateTime.UtcNow
            },
            new() {
                IdPropiedad = 3,
                Nombre = "Cabaña en la montaña",
                Descripcion = "Cabaña acogedora en la montaña",
                Ubicacion = "Valle de Bravo",
                Activa = true,
                FechaCreacion = DateTime.UtcNow
            },
        ];
    }
}