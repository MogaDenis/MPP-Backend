using MPP_Backend.Data.Models;

namespace MPP_Backend.Data.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<Car?> GetCarByIdAsync(int carId);
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<int> AddCarAsync(Car car);
        Task<bool> DeleteCarAsync(int carId);
        Task<bool> UpdateCarAsync(int carId, Car newCar);
    }
}
