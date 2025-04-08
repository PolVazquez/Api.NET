using ApiNET.Data;
using ApiNET.Model;
using ApiNET.Model.Dto.Usuario;
using ApiNET.Repository.IRepository;
using ApiNET.Security;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiNET.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string secretApi;

        public UsuarioRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            secretApi = configuration.GetValue<string>("ApiSettings:secret");
        }

        public AppUsuario GetUsuario(string id)
            => _db.AppUsuarios.FirstOrDefault(u => u.Id == id);

        public ICollection<AppUsuario> GetUsuarios()
            => _db.AppUsuarios.OrderBy(u => u.UserName).ToList();

        public bool IsUniqueUser(string usuario)
            => _db.AppUsuarios.All(u => u.UserName != usuario);

        public async Task<UsuarioLoginResponseDto> Login(UsuarioLoginDto dto)
        {
            // Buscar el usuario en la base de datos por nombre de usuario
            var usuario = await _db.AppUsuarios
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == dto.NombreUsuario.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuario, dto.Password);
            if (usuario == null || !isValid)
            {
                return new UsuarioLoginResponseDto()
                {
                    Token = "",
                    Usuario = null,
                    Role = null
                };
            }

            var roles = await _userManager.GetRolesAsync(usuario);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretApi);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new UsuarioLoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDataDto>(usuario)
            };
        }


        public async Task<UsuarioDataDto> Register(UsuarioRegisterDto dto)
        {
            var appUsuario = new AppUsuario
            {
                UserName = dto.NombreUsuario,
                Email = dto.NombreUsuario,
                NormalizedEmail = dto.NombreUsuario.ToUpper(),
                Nombre = dto.Nombre
            };
            var result = await _userManager.CreateAsync(appUsuario, dto.Password);

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registered"));
                }

                await _userManager.AddToRoleAsync(appUsuario, "admin");
                var resultUser = _db.AppUsuarios.FirstOrDefault(u => u.UserName.ToLower() == dto.NombreUsuario.ToLower());

                return _mapper.Map<UsuarioDataDto>(resultUser);
            }

            return new UsuarioDataDto();
        }
    }
}