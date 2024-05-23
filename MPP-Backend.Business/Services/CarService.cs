using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services.Interfaces;
using MPP_Backend.Data.Models;
using MPP_Backend.Data.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MPP_Backend.Business.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IOwnerService ownerService, IMapper mapper)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _ownerService = ownerService ?? throw new ArgumentNullException(nameof(ownerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> ValidCar(CarForAddUpdateDTO car) 
        {
            if (car.Make.IsNullOrEmpty() || car.Model.IsNullOrEmpty() || car.Colour.IsNullOrEmpty()) 
            {
                return false;
            }

            var owner = await _ownerService.GetOwnerByIdAsync(car.OwnerId);
            if (owner == null)
            {
                return false;
            }

            return true;
        }

        public async Task<int> AddCarAsync(CarForAddUpdateDTO car)
        {
            if (await ValidCar(car) == false) 
            {
                throw new ValidationException("Invalid car data.");
            }

            return await _carRepository.AddCarAsync(_mapper.Map<Car>(car));
        }

        public async Task<bool> DeleteCarAsync(int carId)
        {
            if (carId < 0)
            {
                throw new ValidationException("Invalid car id.");
            }

            return await _carRepository.DeleteCarAsync(carId);
        }

        public async Task<bool> UpdateCarAsync(int carId, CarForAddUpdateDTO newCar)
        {
            if (carId < 0)
            {
                throw new ValidationException("Invalid car id.");
            }

            if (await ValidCar(newCar) == false)
            {
                throw new ValidationException("Invalid car data.");
            }

            return await _carRepository.UpdateCarAsync(carId, _mapper.Map<Car>(newCar));
        }

        public async Task<IEnumerable<CarDTO>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            return _mapper.Map<IEnumerable<CarDTO>>(cars);
        }

        public async Task<IEnumerable<CarDTO>> GetCarsOfOwnerAsync(int ownerId)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(ownerId);
            if (owner == null)
            {
                throw new ValidationException("The given owner does not exist.");
            }

            var carsOfOwner = _ownerService.GetCarsOfOwnerAsync(ownerId);
            return _mapper.Map<IEnumerable<CarDTO>>(carsOfOwner);
        }

        public async Task<CarDTO?> GetCarByIdAsync(int carId)
        {
            if (carId < 0)
            {
                throw new ValidationException("Invalid car id.");
            }

            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null) 
            {
                return null;
            }

            return _mapper.Map<CarDTO>(car);
        }
    }
}
