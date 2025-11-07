using Backend.Auth.Services;
using Backend.features.Auth.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.features.Auth.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO dto)
        {
            var authResult = await _service.LoginAsync(dto);
            return authResult is not null
            ? Ok(authResult)
            : Unauthorized(new { Results = new features.Results.ResultError { errorMessage = "Email o contraseña incorrectos." } });
            
        }
    }
}