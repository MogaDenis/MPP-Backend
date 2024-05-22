using AutoMapper;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services.Interfaces;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;

namespace MPP_Backend.Business.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IOwnerService ownerService, IMapper mapper)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> AddCarAsync(CarForAddUpdateDTO car)
        {
            return await _carRepository.AddCarAsync(_mapper.Map<Car>(car));
        }

        public async Task<bool> DeleteCarAsync(int carId)
        {
            return await _carRepository.DeleteCarAsync(carId);
        }

        public async Task<bool> UpdateCarAsync(int carId, CarForAddUpdateDTO newCarModel)
        {
            return await _carRepository.UpdateCarAsync(carId, _mapper.Map<Car>(newCarModel));
        }

        public async Task<IEnumerable<CarDTO>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllCarsAsync();

            return _mapper.Map<IEnumerable<CarDTO>>(cars);
        }

        public async Task<IEnumerable<CarDTO>> GetCarsOfOwnerAsync(int ownerId)
        {
            var allCars = await _carRepository.GetAllCarsAsync();

            return _mapper.Map<IEnumerable<CarDTO>>(allCars.Where(car => car.OwnerId == ownerId));
        }

        public async Task<CarDTO?> GetCarByIdAsync(int carId)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);

            if (car == null) 
            {
                return null;
            }

            return _mapper.Map<CarDTO>(car);
        }
    }
}
