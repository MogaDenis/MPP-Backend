using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPP_Backend.Business.Models;
using MPP_Backend.Business.Services.Interfaces;

namespace MPP_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Cars")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarController(ICarService carService, IMapper mapper)
        {
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("singleCar/{carId}", Name = "GetCar")]
        public async Task<IActionResult> GetCar(int carId)
        {
            var car = await _carService.GetCarByIdAsync(carId);

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();

            return Ok(cars);
        }

        [HttpGet("{ownerId}")]
        public async Task<IActionResult> GetCarsOfOwner(int ownerId)
        {
            try
            {
                var ownerCars = await _carService.GetCarsOfOwnerAsync(ownerId);
                return Ok(ownerCars);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] CarForAddUpdateModel carModel)
        {
            try
            {
                var id = await _carService.AddCarAsync(carModel);

                var addedCar = _mapper.Map<CarModel>(carModel);
                addedCar.Id = id;

                return CreatedAtRoute(
                    "GetCar",
                    new
                    {
                        ownerId = carModel.OwnerId,
                        carId = id
                    },
                    addedCar
                );
            }
            catch (Exception) 
            {
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpDelete("{carId}")]
        public async Task<IActionResult> DeleteCar(int carId)
        {
            try
            {
                bool deleted = await _carService.DeleteCarAsync(carId);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPut("{carId}")]
        public async Task<IActionResult> UpdateCar(int carId, [FromBody] CarForAddUpdateModel carModel)
        {
            try
            {
                bool updated = await _carService.UpdateCarAsync(carId, carModel);

                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error.");
            }
        }
    }
}
