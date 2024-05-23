using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

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
            try
            {
                var car = await _carService.GetCarByIdAsync(carId);
                if (car == null)
                {
                    return NotFound();
                }

                return Ok(car);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                var cars = await _carService.GetAllCarsAsync();
                return Ok(cars);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("{ownerId}")]
        public async Task<IActionResult> GetCarsOfOwner(int ownerId)
        {
            try
            {
                var ownerCars = await _carService.GetCarsOfOwnerAsync(ownerId);
                return Ok(ownerCars);
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] CarForAddUpdateDTO carModel)
        {
            try
            {
                var id = await _carService.AddCarAsync(carModel);

                var addedCar = _mapper.Map<CarDTO>(carModel);
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
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
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPut("{carId}")]
        public async Task<IActionResult> UpdateCar(int carId, [FromBody] CarForAddUpdateDTO carModel)
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
