using MPP_Backend.Business.DTOs;

namespace MPP_Backend.Business.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<OwnerDTO> AddOwnerAsync(OwnerForAddUpdateDTO owner);
        Task<bool> UpdateOwnerAsync(int ownerId, OwnerForAddUpdateDTO newOwnerData);
        Task<bool> DeleteOwnerAsync(int ownerId);
        Task<OwnerDTO?> GetOwnerByIdAsync(int ownerId);
        Task<IEnumerable<OwnerDTO>> GetAllOwnersAsync();
    }
}
