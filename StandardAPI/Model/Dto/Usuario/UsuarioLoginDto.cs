using System.ComponentModel.DataAnnotations;

namespace ApiCurso.Model.Dto.Usuario
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El Alias de usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El Password de usuario es obligatorio")]
        public string Password { get; set; }
    }
}