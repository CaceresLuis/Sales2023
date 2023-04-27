using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Country,  CountryDto>().ReverseMap();
            CreateMap<Category,  CategoryDto>().ReverseMap();
        }
    }
}
