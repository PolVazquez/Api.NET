using System.ComponentModel.DataAnnotations;

namespace ApiNET.Model.Dto.Categoria
{
    public class CategoriaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [MaxLength(100, ErrorMessage = "El campo Nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}
