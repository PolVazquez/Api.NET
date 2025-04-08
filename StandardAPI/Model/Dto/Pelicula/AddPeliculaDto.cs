using ApiNET.Model.Dto.Pelicula.Common;

namespace ApiNET.Model.Dto.Pelicula
{
    public class AddPeliculaDto
    {
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public string? UrlImagen { get; set; }
        public IFormFile Imagen { get; set; }

        public TipoClasificacion Clasificacion { get; set; }

        public int IdCategoria { get; set; }
    }
}
