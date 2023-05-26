﻿using AutoMapper;
using Sales.Shared.DTOs;
using Sales.API.Data.Entities;

namespace Sales.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<City,  CityDto>().ReverseMap();
            CreateMap<State,  StateDto>().ReverseMap();
            CreateMap<Country,  CountryDto>().ReverseMap();
            CreateMap<Country,  AddCountryUpdateDto>().ReverseMap();
            CreateMap<User,  CreateUserDto>().ReverseMap();
            CreateMap<Category,  CategoryDto>().ReverseMap();
        }
    }
}
