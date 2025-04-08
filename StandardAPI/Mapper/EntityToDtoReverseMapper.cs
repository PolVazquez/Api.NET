using ApiCurso.Model;
using ApiCurso.Model.Dto.Categoria;
using ApiCurso.Model.Dto.Pelicula;
using ApiCurso.Model.Dto.Usuario;
using AutoMapper;

namespace ApiCurso.Mapper
{
    public class EntityToDtoReverseMapper : Profile
    {
        public EntityToDtoReverseMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, AddCategoriaDto>().ReverseMap();

            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<Pelicula, AddPeliculaDto>().ReverseMap();
            CreateMap<Pelicula, UpdatePeliculaDto>().ReverseMap();

            CreateMap<AppUsuario, UsuarioDataDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();

            CreateMap<UsuarioDataDto, UsuarioDto>().ReverseMap();
        }
    }
}