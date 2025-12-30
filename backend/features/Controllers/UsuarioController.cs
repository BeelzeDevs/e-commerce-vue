using Backend.Features.DTOs;
using Backend.Features.Models;
using Backend.Features.Results;
using Backend.Features.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }


        // [Authorize]
        // [HttpGet]
        // public async Task<IActionResult> GetAll()
        // {
        //     var usuarios = await _service.GetAll();
        //     return Ok(new ApiResponse<List<UsuarioReadDTO>>(usuarios));
        // }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllPagerFilterAdmin([FromQuery] UsuarioQueryDTO dto)
        {
            var usuarios = await _service.GetAllPagerFilterAdmin(dto);
            return Ok(new ApiResponse<ResultadoPaginado<UsuarioReadDTO>>(usuarios));
        }

        [Authorize]
        [HttpGet("{UsuarioId}")]
        public async Task<IActionResult> GetById(int UsuarioId)
        {
            var usuario = await _service.GetById(UsuarioId);
            return Ok(new ApiResponse<UsuarioReadDTO>(usuario));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDTO dto)
        {
            var UsuarioCreado = await _service.Create(dto);
            return Ok(new ApiResponse<UsuarioReadDTO>(UsuarioCreado));
        }
        
        [Authorize]
        [HttpPost("crear-admin")]
        public async Task<IActionResult> Create([FromBody] CreateAdminDTO dto)
        {
            var UsuarioCreado = await _service.CreateAdmin(dto);
            return Ok(new ApiResponse<UsuarioReadDTO>(UsuarioCreado));
        }


        [Authorize]
        [HttpPut("{UsuarioId}")]
        public async Task<IActionResult> Update(int UsuarioId, UsuarioUpdateDTO dto)
        {
            var ExitoAlModif = await _service.Update(UsuarioId, dto);
            return Ok(ApiResponse<ResultSuccess>.Success("Usuario modificado con éxito"));
        }

        
        [Authorize]
        [HttpDelete("{UsuarioId}")]
        public async Task<IActionResult> DeleteByLogic(int UsuarioId)
        {
            var BajaExitosa = await _service.DeleteByLogic(UsuarioId);
            return Ok(ApiResponse<ResultSuccess>.Success("Eliminado con éxito"));
        }

    }
}