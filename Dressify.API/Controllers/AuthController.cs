using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dressify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config) => _config = config;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var adminSection = _config.GetSection("AdminUser");
            var adminUser = adminSection.GetValue<string>("Username");
            var adminPass = adminSection.GetValue<string>("Password");

            if (req.Username != adminUser || req.Password != adminPass)
                return Unauthorized(new { message = "Invalid credentials" });

            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expire = jwtSection.GetValue<int>("ExpireMinutes");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(key);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, req.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expire),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { token = jwt });
        }

        public record LoginRequest(string Username, string Password);
    }
}
