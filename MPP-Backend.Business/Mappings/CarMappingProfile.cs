using AutoMapper;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.Mappings
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile() 
        {
            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<CarForAddUpdateDTO, CarDTO>().ReverseMap();
            CreateMap<CarForAddUpdateDTO, Car>().ReverseMap();
        }
    }
}
