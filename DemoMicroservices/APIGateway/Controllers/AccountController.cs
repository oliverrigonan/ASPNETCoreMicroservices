using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGateway.DataContext;
using APIGateway.DTOs;
using APIGateway.Interfaces;
using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountInterface _accountRepository;

        public AccountController(IConfiguration configuration, IAccountInterface accountRepository)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            var user = _accountRepository.GetAccount(login.Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                AuthenticationService _authenticationService = new AuthenticationService(_configuration);
                var token = _authenticationService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public IActionResult Register([FromBody] RegistrationDTO register)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);

            if (_accountRepository.Register(register, hashedPassword) == false)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            var user = _accountRepository.GetAccount(register.Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(register.Password, user.Password))
            {
                AuthenticationService _authenticationService = new AuthenticationService(_configuration);
                var token = _authenticationService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
