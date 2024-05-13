using AutoMapper;
using MPP_Backend.Business.Models;
using MPP_Backend.Business.Services.Interfaces;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;

namespace MPP_Backend.Business.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerService(IOwnerRepository userRepository, IMapper mapper) 
        {
            _ownerRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> AddOwnerAsync(OwnerModel owner)
        {
            return await _ownerRepository.AddOwnerAsync(_mapper.Map<Owner>(owner));
        }

        public async Task<bool> DeleteOwnerAsync(int ownerId)
        {
            return await _ownerRepository.DeleteOwnerAsync(ownerId);
        }

        public async Task<bool> UpdateOwnerAsync(int ownerId, OwnerForAddUpdateModel newOwnerData)
        {
            return await _ownerRepository.UpdateOwnerAsync(ownerId, _mapper.Map<Owner>(newOwnerData));
        }

        public async Task<IEnumerable<OwnerModel>> GetAllOwnersAsync()
        {
            var owners = await _ownerRepository.GetAllOwnersAsync();

            return _mapper.Map<IEnumerable<OwnerModel>>(owners);    
        }

        public async Task<IEnumerable<OwnerModel>> GetAllOwners()
        {
            var owners = await _ownerRepository.GetAllOwnersAsync();

            return _mapper.Map<IEnumerable<OwnerModel>>(owners);
        }

        public async Task<OwnerModel?> GetOwnerByIdAsync(int ownerId)
        {
            var owner = await _ownerRepository.GetOwnerByIdAsync(ownerId);

            return _mapper.Map<OwnerModel>(owner);
        }

        //public async Task<OwnerModel?> GetOwnerWithCarsAsync(int ownerId)
        //{
        //    var owner = await _ownerRepository.GetOwnerWithCarsAsync(ownerId);

        //    return _mapper.Map<OwnerModel>(owner);
        //}
    }
}
