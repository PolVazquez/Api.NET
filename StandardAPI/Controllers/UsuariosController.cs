using ApiNET.Model;
using ApiNET.Model.Dto.Usuario;
using ApiNET.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiNET.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/v{version:apiVersion}/usuarios")]
    [ApiController]
    [ApiVersionNeutral]
    public class UsuariosController(IUsuarioRepository usuarioRepository, IMapper mapper) : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly IMapper _mapper = mapper;
        protected ResponseAPI _responseAPI = new();

        // Obtener todos los usuarios
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("CorsPolicy")]  //Aplica políticas solo a este endpoint
        public IActionResult GetUsuarios()
        {
            try
            {
                var listaUsuarios = _usuarioRepository.GetUsuarios();
                var dtos = new List<UsuarioDto>();
                foreach (var item in listaUsuarios)
                {
                    dtos.Add(_mapper.Map<UsuarioDto>(item));
                }
                _responseAPI.Result = dtos;
                _responseAPI.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _responseAPI.Success = false;
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.ErrorMessages.Add(ex.Message);
            }
            return StatusCode((int)_responseAPI.StatusCode, _responseAPI);
        }

        // Obtener un usuario por ID
        [HttpGet("{id}", Name = "GetUsuario")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EnableCors("CorsPolicy")]  //Aplica políticas solo a este endpoint
        public IActionResult GetUsuario(string id)
        {
            try
            {
                var usuario = _usuarioRepository.GetUsuario(id);
                if (usuario == null)
                {
                    _responseAPI.Success = false;
                    _responseAPI.StatusCode = HttpStatusCode.NotFound;
                    _responseAPI.ErrorMessages.Add("Usuario no encontrado");
                    return NotFound(_responseAPI);
                }

                _responseAPI.Result = _mapper.Map<UsuarioDto>(usuario);
                _responseAPI.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _responseAPI.Success = false;
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.ErrorMessages.Add(ex.Message);
            }
            return StatusCode((int)_responseAPI.StatusCode, _responseAPI);
        }

        // Registrar un usuario nuevo
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UsuarioRegisterDto dto)
        {
            try
            {
                if (!_usuarioRepository.IsUniqueUser(dto.NombreUsuario))
                {
                    _responseAPI.Success = false;
                    _responseAPI.StatusCode = HttpStatusCode.BadRequest;
                    _responseAPI.ErrorMessages.Add("El usuario ya existe.");
                    return BadRequest(_responseAPI);
                }

                var usuario = await _usuarioRepository.Register(dto);
                if (usuario == null)
                {
                    _responseAPI.Success = false;
                    _responseAPI.StatusCode = HttpStatusCode.BadRequest;
                    _responseAPI.ErrorMessages.Add("Error al registrar usuario.");
                    return BadRequest(_responseAPI);
                }

                _responseAPI.Result = _mapper.Map<UsuarioDto>(usuario);
                _responseAPI.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetUsuario", new { id = usuario.Id }, _responseAPI);
            }
            catch (Exception ex)
            {
                _responseAPI.Success = false;
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.ErrorMessages.Add(ex.Message);
            }
            return StatusCode((int)_responseAPI.StatusCode, _responseAPI);
        }

        // Login de usuario
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
        {
            try
            {
                var response = await _usuarioRepository.Login(dto);
                if (response == null || string.IsNullOrEmpty(response.Token))
                {
                    _responseAPI.Success = false;
                    _responseAPI.StatusCode = HttpStatusCode.Unauthorized;
                    _responseAPI.ErrorMessages.Add("Credenciales incorrectas");
                    return Unauthorized(_responseAPI);
                }

                _responseAPI.Result = response;
                _responseAPI.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _responseAPI.Success = false;
                _responseAPI.StatusCode = HttpStatusCode.InternalServerError;
                _responseAPI.ErrorMessages.Add(ex.Message);
            }
            return StatusCode((int)_responseAPI.StatusCode, _responseAPI);
        }
    }
}
