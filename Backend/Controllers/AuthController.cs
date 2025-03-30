using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // This should be replaced with your actual user authentication logic
            if ((login.Username == "admin" && login.Password == "admin") ||
                (login.Username == "user" && login.Password == "user")) // Example check for admin or simple user
            {
                var role = login.Username == "admin" ? "admin" : "user"; // Set role based on the username
                var token = GenerateJwtToken(login.Username, role); // Pass the role to the token generator
                return Ok(new { token, role }); // Return token and role in the response
            }
            else
            {
                return Unauthorized("Invalid credentials"); // If the credentials don't match, return Unauthorized
            }
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role), // Example: add a role claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
