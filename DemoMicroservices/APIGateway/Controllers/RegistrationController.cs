using System;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using APIGateway.Models;
using Microsoft.EntityFrameworkCore;
using APIGateway.DataContext;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GatewayDBContext _dbContext;

        public RegistrationController(
            IConfiguration configuration,
            GatewayDBContext dbContext
        ) {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Register([FromBody] RegistrationModel model)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            UserModel newUser = new UserModel()
            {
                Username = model.UserName,
                Email = model.Email,
                Password = hashedPassword
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            var token = GenerateJwtToken(newUser);

            return Ok(new { Token = token });
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
