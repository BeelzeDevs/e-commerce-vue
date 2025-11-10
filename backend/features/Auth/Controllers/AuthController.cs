using Backend.Features.Auth.Services;
using Backend.Features.Auth.DTOs;
using Microsoft.AspNetCore.Mvc;
using Backend.Features.Results;

namespace Backend.Features.Auth.Controllers
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
            : Unauthorized(ApiResponse<ResultError>.Error("Email o contraseña incorrectos."));
            
        }
    }
}