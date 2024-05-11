using MPP_Backend.Business.Models;

namespace MPP_Backend.Business.Services.Interfaces
{
    public interface ICarService
    {
        Task<CarModel?> GetCarByIdAsync(int carId);
        Task<IEnumerable<CarModel>> GetAllCarsAsync();
        Task<IEnumerable<CarModel>> GetCarsOfOwnerAsync(int ownerId);
        Task<int> AddCarAsync(CarForAddUpdateModel car);
        Task<bool> DeleteCarAsync(int carId);
        Task<bool> UpdateCarAsync(int carId, CarForAddUpdateModel newCarModel);
    }
}
