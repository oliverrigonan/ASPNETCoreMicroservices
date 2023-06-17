using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIGateway.Models;
using Microsoft.EntityFrameworkCore;
using APIGateway.DataContext;

namespace YourApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GatewayDBContext _dbContext;

        public LoginController(
            IConfiguration configuration,
            GatewayDBContext dbContext
        )
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _dbContext.Users.Where(d => d.Username == model.Username).FirstOrDefault();

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }

        private String GenerateJwtToken(UserModel user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<String>("SecretKey");
            var issuer = jwtSettings.GetValue<String>("Issuer");
            var audience = jwtSettings.GetValue<String>("Audience");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
