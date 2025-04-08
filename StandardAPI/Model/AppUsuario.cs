using Microsoft.AspNetCore.Identity;

namespace ApiCurso.Model
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}