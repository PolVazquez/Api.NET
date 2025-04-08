using AutoMapper;

namespace MinimalAPI.AutoMapper
{
    public class MapperDTOs : Profile
    {
        public MapperDTOs()
        {
            CreateMap<Models.Propiedad, Models.DTOs.AddPropiedadDTO>().ReverseMap();

            CreateMap<Models.Propiedad, Models.DTOs.PropiedadDTO>().ReverseMap();
        }
    }
}