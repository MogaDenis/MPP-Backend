using MPP_Backend.Business.Models;

namespace MPP_Backend.Business.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<int> AddOwnerAsync(OwnerModel owner);
        Task<bool> UpdateOwnerAsync(int ownerId, OwnerForAddUpdateModel newOwnerData);
        Task<bool> DeleteOwnerAsync(int ownerId);
        Task<OwnerModel?> GetOwnerByIdAsync(int ownerId);
        //Task<OwnerModel?> GetOwnerWithCarsAsync(int ownerId);   
        Task<IEnumerable<OwnerModel>> GetAllOwnersAsync();
    }
}
