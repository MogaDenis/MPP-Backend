using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MPP_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IUserService userService, IConfiguration configuration) 
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService)); 
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));   
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            bool loggedIn = await _userService.CheckUserLoginRequest(loginRequest);

            if (!loggedIn) 
            {
                return NotFound();
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return Ok(jwtToken);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForAddUpdateDTO user)
        {
            var registeredUser = await _userService.AddUserAsync(user);

            if (registeredUser == null) 
            {
                return BadRequest();
            }

            return NoContent(); 
        }
    }
}
