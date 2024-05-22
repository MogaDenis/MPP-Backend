using MPP_Backend.Business.DTOs;

namespace MPP_Backend.Business.Services.Interfaces
{
    public interface ICarService
    {
        Task<CarDTO?> GetCarByIdAsync(int carId);
        Task<IEnumerable<CarDTO>> GetAllCarsAsync();
        Task<IEnumerable<CarDTO>> GetCarsOfOwnerAsync(int ownerId);
        Task<int> AddCarAsync(CarForAddUpdateDTO car);
        Task<bool> DeleteCarAsync(int carId);
        Task<bool> UpdateCarAsync(int carId, CarForAddUpdateDTO newCarModel);
    }
}
