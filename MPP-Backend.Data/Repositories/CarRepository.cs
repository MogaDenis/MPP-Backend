using Microsoft.EntityFrameworkCore;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;

namespace MPP_Backend.Data.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarManagerContext _context;

        public CarRepository(CarManagerContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return car.Id;
        }

        public async Task<bool> DeleteCarAsync(int carId)
        {
            var carToDelete = await GetCarByIdAsync(carId);

            if (carToDelete == null) 
            {
                return false;
            }

            _context.Cars.Remove(carToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int carId)
        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
        }

        public async Task<bool> UpdateCarAsync(int carId, Car newCar)
        {
            var carToUpdate = await GetCarByIdAsync(carId);

            if (carToUpdate == null)
            {
                return false;
            }

            newCar.Id = carId;
            _context.Cars.Entry(carToUpdate).CurrentValues.SetValues(newCar);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
