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
    public class OrdenesController : ControllerBase
    {
        private readonly IOrdenService _service;
        public OrdenesController(IOrdenService service)
        {
            _service = service;
        }

        [Authorize(Policy = "SoloAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordenes = await _service.GetAll();
            return Ok(new ApiResponse<List<OrdenReadDTO>>(ordenes));

        }
        
        [Authorize]
        [HttpGet("{OrdenId}")]
        public async Task<IActionResult> GetByOrdenId(int OrdenId)
        {
            var orden = await _service.GetByOrdenId(OrdenId);
            return Ok(new ApiResponse<OrdenReadDTO>(orden));

        }
        
        [Authorize]
        [HttpGet("usuario/{UsuarioId}/all")]
        public async Task<IActionResult> GetOrdenesByUserId(int UsuarioId)
        {
            var ordenes = await _service.GetOrdenesByUsarioId(UsuarioId);
            return Ok(new ApiResponse<List<OrdenReadDTO>>(ordenes));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrdenCreateDTO dto)
        {
            var OrdenCreada = await _service.Create(dto);
            return CreatedAtAction(nameof(GetByOrdenId), new { OrdenId = OrdenCreada.Id }, new {Results = OrdenCreada});
        }
        // admines y usuarios que tengan el id del token
        [Authorize]
        [HttpPut("{OrdenId}")]
        public async Task<IActionResult> Update(int OrdenId, [FromBody] OrdenUpdateDTO dto)
        {
            var OrdenActualizadaConExito = await _service.Update(OrdenId, dto);
            return Ok(ApiResponse<ResultSuccess>.Success("Orden actualizada con éxito"));
            
        }
        // admines y usuarios que tengan el id del token
        [Authorize]
        [HttpDelete("{OrdenId}")]
        public async Task<IActionResult> Delete(int OrdenId)
        {
            var exito = await _service.Delete(OrdenId);
            return Ok(ApiResponse<ResultSuccess>.Success("Se elimino con éxito"));
        }
        // admines y usuarios que tengan el id del token
        [Authorize]
        [HttpGet("{OrdenId}/detalles")]
        public async Task<IActionResult> GetDetallesByOrdenId(int OrdenId)
        {
            var detalles = await _service.GetDetallesByOrdenId(OrdenId);
            return Ok(new ApiResponse<List<DetalleReadDTO>>(detalles));
        }
        // admines y usuarios que tengan el id del token
        [Authorize]
        [HttpPut("addDetalle")]
        public async Task<IActionResult> AddDetalle([FromBody] DetalleCreateDTO dto)
        {
            var DetalleAgregado = await _service.AddDetalle(dto);
            return Ok(ApiResponse<ResultSuccess>.Success("Producto agregado con éxito."));
        }
        // admines y usuarios que tengan el id del token
        [Authorize]
        [HttpDelete("removeDetalle")]
        public async Task<IActionResult> DeleteDetalle([FromBody] DetalleDeleteDTO dto)
        {
            var DetalleEliminado = await _service.DeleteDetalle(dto);
            return Ok(ApiResponse<ResultSuccess>.Success("Producto eliminado con éxito."));
        }
        // admines y usuarios que tengan el id del token
        [Authorize]
        [HttpPut("updateDetalle")]
        public async Task<IActionResult> UpdateDetalle([FromBody] DetalleCreateDTO dto)
        {
            var DetalleModificado = await _service.UpdateDetalle(dto);
            return Ok(ApiResponse<ResultSuccess>.Success("Producto modificado con éxito."));
        }

    }
}