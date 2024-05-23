using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MPP_Backend.Business.DTOs;
using MPP_Backend.Business.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            try
            {
                bool loggedIn = await _userService.CheckUserLoginRequest(loginRequest);
                if (!loggedIn)
                {
                    return NotFound();
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims = [];

                var user = await _userService.GetUserByEmail(loginRequest.Email);
                if (user != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
                }

                var securityToken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
                return Ok(jwtToken);
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

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForAddUpdateDTO user)
        {
            try
            {
                var registeredUser = await _userService.AddUserAsync(user);

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

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpGet("CheckIfTokenExpired")]
        public IActionResult CheckIfTokenExpired()
        {
            var token = HttpContext.Request.Headers.Authorization.ToString()[7..];
            JwtSecurityToken jwtToken = new (token);

            var exp = jwtToken.Claims.First(x => x.Type == "exp").Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp));

            if (expTime < DateTimeOffset.UtcNow)
            {
                return Unauthorized();
            }

            return Ok();
        }
    }
}
