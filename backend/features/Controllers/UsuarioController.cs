using Backend.features.DTOs;
using Backend.features.Models;
using Backend.features.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.features.Controllers
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


        [Authorize(Policy ="SoloAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _service.GetAll();
            return usuarios is not null
            ? Ok(new { Results = usuarios })
            : NotFound(new
            {
                Results = new features.Results.ResultError { errorMessage = "No se encontraron usuarios" }
            });
        }

        [Authorize(Policy ="SoloAdmin")]
        [HttpGet("{UsuarioId}")]
        public async Task<IActionResult> GetById(int UsuarioId)
        {
            var usuario = await _service.GetById(UsuarioId);
            return usuario is not null 
            ? Ok(new { Results = usuario }) 
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se encontró usuario con el Usuario ID : {UsuarioId}" } 
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDTO dto)
        {
            var UsuarioCreado = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { Id = UsuarioCreado.Id }, UsuarioCreado);
        }

        [HttpPut("{UsuarioId}")]
        public async Task<IActionResult> Update(int UsuarioId, UsuarioCreateDTO dto)
        {
            var ExitoAlModif = await _service.Update(UsuarioId, dto);
            return ExitoAlModif 
            ? Ok(new { Results = new features.Results.ResultSuccess {successMessage = "Usuario modificado con éxito"} }) 
            : NotFound( new { Results = new features.Results.ResultError { errorMessage = $"No se pudo actualizar el usuario a modificar, datos incorrectos UsuarioId = {UsuarioId} , \n UsuarioCreateDTO = {dto}"} 
            });
        }

        [Authorize(Policy ="SoloAdmin")]
        [HttpDelete("{UsuarioId}")]
        public async Task<IActionResult> DeleteByLogic(int UsuarioId)
        {
            var BajaExitosa = await _service.DeleteByLogic(UsuarioId);
            return BajaExitosa 
            ? Ok( new { Results = new features.Results.ResultSuccess { successMessage = "Eliminado con éxito" }  }) 
            : NotFound( new { Results = new features.Results.ResultError { errorMessage = $"No se encontró el usuario, UsuarioId = {UsuarioId}" } 
            });
        }

    }
}