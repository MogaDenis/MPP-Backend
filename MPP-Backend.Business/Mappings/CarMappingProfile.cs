using AutoMapper;
using MPP_Backend.Business.Models;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.Mappings
{
    public class CarMappingProfile : Profile
    {
        public CarMappingProfile() 
        {
            CreateMap<Car, CarModel>().ReverseMap();
            CreateMap<CarForAddUpdateModel, CarModel>().ReverseMap();
            CreateMap<CarForAddUpdateModel, Car>().ReverseMap();
        }
    }
}
