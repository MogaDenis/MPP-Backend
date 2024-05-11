using AutoMapper;
using MPP_Backend.Business.Models;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.Mappings
{
    public class OwnerMappingProfile : Profile
    {
        public OwnerMappingProfile()
        {
            CreateMap<Owner, OwnerModel>().ReverseMap();
            CreateMap<OwnerForAddUpdateModel, OwnerModel>().ReverseMap();
            CreateMap<OwnerForAddUpdateModel, Owner>().ReverseMap();
        }
    }
}
