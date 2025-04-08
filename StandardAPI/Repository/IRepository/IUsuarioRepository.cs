using ApiNET.Model;
using ApiNET.Model.Dto.Usuario;

namespace ApiNET.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<AppUsuario> GetUsuarios();
        AppUsuario GetUsuario(string id);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginResponseDto> Login(UsuarioLoginDto dto);
        Task<UsuarioDataDto> Register(UsuarioRegisterDto dto);
    }
}