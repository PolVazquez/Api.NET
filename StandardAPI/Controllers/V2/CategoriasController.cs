using ApiNET.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiNET.Controllers.V2
{
    [Authorize(Roles = "admin")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/categorias")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]

    public class CategoriasController(ICategoriaRepository categoriaRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
        private readonly IMapper _mapper = mapper;

        [AllowAnonymous]
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "string1", "string2" };
        }
    }
}