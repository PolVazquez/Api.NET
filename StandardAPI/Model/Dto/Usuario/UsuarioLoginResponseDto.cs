namespace ApiCurso.Model.Dto.Usuario
{
    public class UsuarioLoginResponseDto
    {
        public UsuarioDataDto Usuario { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}