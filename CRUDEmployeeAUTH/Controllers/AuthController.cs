using CrudEmployeeAUTH.Models;
using CRUDEmployeeAUTH.ActionFilters;
using CRUDEmployeeAUTH.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrudEmployeeAUTH.Controllers
{
    [ServiceFilter(typeof(LoggingFilter))]
    [ServiceFilter(typeof(ExceptionHandlingFilter))]
   //[ServiceFilter(typeof(ValidationFilter))]
    [ServiceFilter(typeof(ResultFilter))]
    [ServiceFilter(typeof(CacheFilter))]
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(userRegister.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var user = new User
            {
                Email = userRegister.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password),
                Company = userRegister.Company 
            };

            await _userRepository.AddAsync(user);
            return Ok("User registered successfully.");
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserLogin userLogin)
        {
            var user = await _userRepository.GetByUsernameAsync(userLogin.Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
            {
                var token = GenerateToken(user);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials.");
        }

        private string GenerateToken(User user)
        {
            var key = _config["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Email),
        new Claim("Company", user.Company) 
    };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:Expires"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
