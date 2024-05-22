using AutoMapper;
using MPP_Backend.Business.DTOs;
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

        public async Task<OwnerDTO> AddOwnerAsync(OwnerForAddUpdateDTO owner)
        {
            int newOwnerId = await _ownerRepository.AddOwnerAsync(_mapper.Map<Owner>(owner));

            var newOwner = _mapper.Map<OwnerDTO>(owner);
            newOwner.Id = newOwnerId;

            return newOwner;
        }

        public async Task<bool> DeleteOwnerAsync(int ownerId)
        {
            return await _ownerRepository.DeleteOwnerAsync(ownerId);
        }

        public async Task<bool> UpdateOwnerAsync(int ownerId, OwnerForAddUpdateDTO newOwnerData)
        {
            return await _ownerRepository.UpdateOwnerAsync(ownerId, _mapper.Map<Owner>(newOwnerData));
        }

        public async Task<IEnumerable<OwnerDTO>> GetAllOwnersAsync()
        {
            var owners = await _ownerRepository.GetAllOwnersAsync();

            return _mapper.Map<IEnumerable<OwnerDTO>>(owners);    
        }

        public async Task<IEnumerable<OwnerDTO>> GetAllOwners()
        {
            var owners = await _ownerRepository.GetAllOwnersAsync();

            return _mapper.Map<IEnumerable<OwnerDTO>>(owners);
        }

        public async Task<OwnerDTO?> GetOwnerByIdAsync(int ownerId)
        {
            var owner = await _ownerRepository.GetOwnerByIdAsync(ownerId);

            return _mapper.Map<OwnerDTO>(owner);
        }
    }
}
