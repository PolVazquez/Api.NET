using ApiCurso.Model;
using ApiCurso.Model.Dto.Categoria;
using ApiCurso.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCurso.Controllers.V1
{
    [Authorize(Roles = "admin")]
    [Route("api/v{version:apiVersion}/categorias")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class CategoriasController(ICategoriaRepository categoriaRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
        private readonly IMapper _mapper = mapper;

        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _categoriaRepository.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();
            foreach (var lista in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriasDto);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        [HttpGet("{id:int}", Name = "GetCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int id)
        {
            var categoria = _categoriaRepository.GetCategoria(id);
            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaDto = _mapper.Map<CategoriaDto>(categoria);
            return Ok(categoriaDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddCategoria([FromBody] AddCategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest(ModelState);

            if (_categoriaRepository.ExistsCategoria(dto.Nombre))
            {
                ModelState.AddModelError("", $"La categoria {dto.Nombre} ya existe");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(dto);
            if (!_categoriaRepository.CreateCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {categoria.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { id = categoria.Id }, categoria);
        }

        [HttpPatch("{id:int}", Name = "UpdatePatchCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchCategoria(int id, [FromBody] CategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null || dto.Id != id)
                return BadRequest(ModelState);

            var categoria = _mapper.Map<Categoria>(dto);
            if (!_categoriaRepository.UpdateCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdatePutCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePutCategoria(int id, [FromBody] CategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null || dto.Id != id)
                return BadRequest(ModelState);

            var existe = _categoriaRepository.ExistsCategoria(id);
            if (!existe)
            {
                ModelState.AddModelError("", $"La categoria {dto.Nombre} no existe");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(dto);
            if (!_categoriaRepository.UpdateCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategoria(int id)
        {
            if (!_categoriaRepository.ExistsCategoria(id))
                return NotFound();

            var categoria = _categoriaRepository.GetCategoria(id);
            if (!_categoriaRepository.DeleteCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el registro {categoria.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("GetString")]
        [Obsolete("Este método está obsoleto. Use el endpoint GetString en la versión 2.0.")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}