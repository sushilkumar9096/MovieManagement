using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.WebAPI.DTOs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto dto)
        {
            if (dto == null) return BadRequest("Invalid request");

            // Check if user exists
            var existingUser = _unitOfWork.Users.GetByUsername(dto.Username);
            if (existingUser != null)
            {
                return BadRequest("Username is already taken.");
            }

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = MovieManagement.Domain.Entities.User.HashPassword(dto.Password),
                Role = dto.Role
            };

            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto dto)
        {
            if (dto == null) return BadRequest("Invalid request");

            var user = _unitOfWork.Users.GetByUsername(dto.Username);
            if (user == null || !user.VerifyPassword(dto.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token = token, username = user.Username, role = user.Role });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyStr = jwtSettings["Key"] ?? "SuperSecretKeyForMovieManagementAPISecurityMustBeAtLeast32BytesLong!";
            var key = Encoding.UTF8.GetBytes(keyStr);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtSettings["Issuer"] ?? "MovieManagementAPI",
                Audience = jwtSettings["Audience"] ?? "MovieManagementClients",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
