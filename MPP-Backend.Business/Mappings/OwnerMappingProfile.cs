using AutoMapper;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.Mappings
{
    public class OwnerMappingProfile : Profile
    {
        public OwnerMappingProfile()
        {
            CreateMap<Owner, OwnerDTO>().ReverseMap();
            CreateMap<OwnerForAddUpdateDTO, OwnerDTO>().ReverseMap();
            CreateMap<OwnerForAddUpdateDTO, Owner>().ReverseMap();
        }
    }
}
