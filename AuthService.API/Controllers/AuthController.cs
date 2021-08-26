using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroService.Interfaces;
using AuthMicroService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger,IAuthService authService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User user)
        {
            if(!string.IsNullOrEmpty(user.EmailId) && !string.IsNullOrEmpty(user.Password))
            {
                var validUser = _authService.MatchUserCredentials(user);
                if(validUser)
                {
                    return Ok();
                }
                else
                {
                    return NotFound("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
