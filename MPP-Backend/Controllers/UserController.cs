using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services.Interfaces;
using MPP_Backend.Data.Models;

namespace MPP_Backend.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("email/{userEmail}")]
        public async Task<IActionResult> GetUserByEmail(string userEmail)
        {
            try
            {
                var user = await _userService.GetUserByEmail(userEmail);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("role/{userRole}")]
        public async Task<IActionResult> FilterUsersByRole(UserRole userRole)
        {
            try
            {
                var users = await _userService.FilterUsersByRole(userRole);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserForAddUpdateDTO user)
        {
            try
            {
                var addedUser = await _userService.AddUserAsync(user);
                if (addedUser == null)
                {
                    return BadRequest();
                }

                return CreatedAtRoute(
                    "GetUser",
                    new
                    {
                        userId = addedUser.Id
                    },
                    addedUser
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                bool deleted = await _userService.DeleteUserAsync(userId);

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

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserForAddUpdateDTO newUser)
        {
            try
            {
                bool updated = await _userService.UpdateUserAsync(userId, newUser);

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
