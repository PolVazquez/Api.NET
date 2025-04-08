using ApiNET.Model;
using ApiNET.Model.Dto.Categoria;
using ApiNET.Model.Dto.Pelicula;
using ApiNET.Model.Dto.Usuario;
using AutoMapper;

namespace ApiNET.Mapper
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