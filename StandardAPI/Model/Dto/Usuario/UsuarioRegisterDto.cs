using System.ComponentModel.DataAnnotations;

namespace ApiNET.Model.Dto.Usuario
{
    public class UsuarioRegisterDto
    {
        [Required(ErrorMessage = "El Alias de usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El Nombre de usuario es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Password de usuario es obligatorio")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}