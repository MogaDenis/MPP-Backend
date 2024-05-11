using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPP_Backend.Business.Models;
using MPP_Backend.Business.Services.Interfaces;

namespace MPP_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Owners")]
    public class OwnerController : Controller
    {
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerService ownerService, IMapper mapper) 
        {
            _ownerService = ownerService ?? throw new ArgumentNullException(nameof(ownerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            try
            {
                return Ok(await _ownerService.GetAllOwnersAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{ownerId}", Name = "GetOwner")]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            try
            {
                var owner = await _ownerService.GetOwnerByIdAsync(ownerId);

                if (owner == null)
                {
                    return NotFound();
                }

                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{ownerId}/allCars")]
        public async Task<IActionResult> GetOwnerWithCars(int ownerId)
        {
            try
            {
                var owner = await _ownerService.GetOwnerWithCarsAsync(ownerId);

                if (owner == null)
                {
                    return NotFound();
                }

                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOwner([FromBody] OwnerModel ownerModel)
        {
            try
            {
                var id = await _ownerService.AddOwnerAsync(ownerModel);

                var addedOwner = _mapper.Map<OwnerModel>(ownerModel);
                addedOwner.Id = id;

                return CreatedAtRoute(
                    "GetOwner",
                    new
                    {
                        ownerId = id
                    },
                    addedOwner
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{ownerId}")]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {
            try
            {
                bool deleted = await _ownerService.DeleteOwnerAsync(ownerId);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{ownerId}")]
        public async Task<IActionResult> UpdateCar(int ownerId, [FromBody] OwnerForAddUpdateModel ownerModel)
        {
            try
            {
                bool updated = await _ownerService.UpdateOwnerAsync(ownerId, ownerModel);

                if (!updated)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
