using Microsoft.EntityFrameworkCore;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;

namespace MPP_Backend.Data.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly CarManagerContext _context;

        public OwnerRepository(CarManagerContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> AddOwnerAsync(Owner owner)
        {
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            return owner.Id;
        }

        public async Task<bool> UpdateOwnerAsync(int ownerId, Owner newOwnerData)
        {
            var ownerToUpdate = await GetOwnerByIdAsync(ownerId);

            if (ownerToUpdate == null)
            {
                return false;
            }

            newOwnerData.Id = ownerId;

            _context.Owners.Entry(ownerToUpdate).CurrentValues.SetValues(newOwnerData);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOwnerAsync(int ownerId)
        {
            var ownerToDelete = await GetOwnerByIdAsync(ownerId);

            if (ownerToDelete == null)
            {
                return false;
            }

            _context.Owners.Remove(ownerToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Owner?> GetOwnerByIdAsync(int ownerId)
        {
            return await _context.Owners.FirstOrDefaultAsync(x => x.Id == ownerId);
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsOfOwnerAsync(int ownerId)
        {
            return await _context.Cars.Where(car => car.OwnerId == ownerId).ToListAsync();
        }
    }
}
