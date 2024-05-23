using MPP_Backend.Data.Models;

namespace MPP_Backend.Data.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        Task<int> AddOwnerAsync(Owner owner);
        Task<bool> UpdateOwnerAsync(int ownerId, Owner newOwnerData);
        Task<bool> DeleteOwnerAsync(int ownerId);
        Task<Owner?> GetOwnerByIdAsync(int ownerId);
        Task<IEnumerable<Owner>> GetAllOwnersAsync();
        Task<IEnumerable<Car>> GetCarsOfOwnerAsync(int ownerId);
    }
}
