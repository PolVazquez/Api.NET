using ApiCurso.Model;
using ApiCurso.Model.Dto.Pelicula;
using ApiCurso.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCurso.Controllers.V2
{
    [Authorize(Roles = "admin")]
    [Route("api/v{version:apiVersion}/peliculas")]
    [ApiController]
    [ApiVersion("2.0")]
    public class PeliculasController(IPeliculaRepository peliculaRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPeliculaRepository _peliculasRepository = peliculaRepository;

        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetPeliculas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var total = _peliculasRepository.GetTotalPeliculas();
                var listaPeliculas = _peliculasRepository.GetPeliculas(pageNumber, pageSize);

                if(listaPeliculas == null || !listaPeliculas.Any())
                {
                    return NotFound("No se encontraron películas.");
                }
                var peliculasDto = listaPeliculas.Select(p => _mapper.Map<PeliculaDto>(p)).ToList();

                var response = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = Math.Ceiling(total / (double) pageSize),
                    TotalItems = total,
                    Items = peliculasDto
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}