using MPP_Backend.Business.DTOs;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<OwnerDTO> AddOwnerAsync(OwnerForAddUpdateDTO owner);
        Task<bool> UpdateOwnerAsync(int ownerId, OwnerForAddUpdateDTO newOwnerData);
        Task<bool> DeleteOwnerAsync(int ownerId);
        Task<OwnerDTO?> GetOwnerByIdAsync(int ownerId);
        Task<IEnumerable<OwnerDTO>> GetAllOwnersAsync();
        Task<IEnumerable<CarDTO>> GetCarsOfOwnerAsync(int ownerId);
    }
}
