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
    [Route("api/Owners")]
    public class OwnerController : Controller
    {
        private readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService) 
        {
            _ownerService = ownerService ?? throw new ArgumentNullException(nameof(ownerService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            try
            {
                var owners = await _ownerService.GetAllOwnersAsync();
                return Ok(owners);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOwner([FromBody] OwnerForAddUpdateDTO ownerModel)
        {
            try
            {
                var addedOwner = await _ownerService.AddOwnerAsync(ownerModel);

                return CreatedAtRoute(
                    "GetOwner",
                    new
                    {
                        ownerId = addedOwner.Id
                    },
                    addedOwner
                );
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{ownerId}")]
        public async Task<IActionResult> UpdateCar(int ownerId, [FromBody] OwnerForAddUpdateDTO ownerModel)
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
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
