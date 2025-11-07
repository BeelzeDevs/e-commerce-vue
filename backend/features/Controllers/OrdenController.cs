using Backend.features.DTOs;
using Backend.features.Models;
using Backend.features.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.features.Controllers
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
        
        [Authorize(Policy ="SoloAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ordenes = await _service.GetAll();
            return ordenes is not null
            ? Ok(new { Results = ordenes })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = "No se encontraron ordenes" } });
        }

        [HttpGet("{OrdenId}")]
        public async Task<IActionResult> GetByOrdenId(int OrdenId)
        {
            var orden = await _service.GetById(OrdenId);
            return orden is not null
            ? Ok(new { Results = orden })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se encontró orden con el Orden ID : {OrdenId}" } });
        }

        [HttpGet("usuario/{UsuarioId}/all")]
        public async Task<IActionResult> GetOrdenesByUserId(int UsuarioId)
        {
            try
            {
                var ordenes = await _service.GetOrdenesByUsarioId(UsuarioId);
                return Ok(new { Results = ordenes });
            }
            catch (System.Exception err)
            {
                return NotFound(new { Results = new features.Results.ResultError { errorMessage = err.Message } });
            }
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrdenCreateDTO dto)
        {
            var OrdenCreada = await _service.Create(dto);
            if (OrdenCreada is null) return NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se pudo crear la orden, DTO : {dto}" } });
            return CreatedAtAction(nameof(GetByOrdenId), new { Id = OrdenCreada.Id }, OrdenCreada);
        }

        [HttpPut("{OrdenId}")]
        public async Task<IActionResult> Update(int OrdenId, [FromBody] OrdenCreateDTO dto)
        {
            var OrdenActualizadaConExito = await _service.Update(OrdenId, dto);
            return OrdenActualizadaConExito
            ? Ok(new { Results = new features.Results.ResultSuccess { successMessage = "Orden actualizada con éxito" } })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se pudo actualizar la Orden con Orden ID : {OrdenId}" } });
        }

        [HttpDelete("{OrdenId}")]
        public async Task<IActionResult> Delete(int OrdenId)
        {
            var exito = await _service.Delete(OrdenId);
            return exito
            ? Ok(new { Results = new features.Results.ResultSuccess { successMessage = "Se elimino con éxito" } })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se pudo eliminar la orden con el Orden ID : {OrdenId}" } });
        }

        [HttpGet("{OrdenId}/detalles")]
        public async Task<IActionResult> GetDetallesByOrdenId(int OrdenId)
        {
            try
            {
                var detalles = await _service.GetDetallesByOrdenId(OrdenId);
                return Ok(new { Results = detalles });
            }
            catch (System.Exception err)
            {
                return NotFound(new { Results = new features.Results.ResultError { errorMessage = err.Message } });
            }
        }
        
        [HttpPut("addDetalle")]
        public async Task<IActionResult> AddDetalle([FromBody] DetalleCreateDTO dto)
        {
            var DetalleAgregado = await _service.AddDetalle(dto);
            return DetalleAgregado
            ? Ok(new { Results = new features.Results.ResultSuccess { successMessage = "Producto agregado con éxito." } })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se pudo agregar el producto a la orden" } });
        }
        [HttpDelete("removeDetalle")]
        public async Task<IActionResult> DeleteDetalle([FromBody] DetalleDeleteDTO dto)
        {
            var DetalleEliminado = await _service.DeleteDetalle(dto);
            return DetalleEliminado
            ? Ok(new { Results = new features.Results.ResultSuccess { successMessage = "Producto eliminado con éxito." } })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se pudo eliminar el producto con el Producto ID : {dto.ProductoId} de la Orden ID : {dto.OrdenId}" } });
        }
        [HttpPut("updateDetalle")]
        public async Task<IActionResult> UpdateDetalle([FromBody] DetalleCreateDTO dto)
        {
            var DetalleModificado = await _service.UpdateDetalle(dto);
            return DetalleModificado
            ? Ok(new { Results = new features.Results.ResultSuccess { successMessage = "Producto modificado con éxito." } })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se pudo modificar el producto con el Producto ID : {dto.ProductoId} de la Orden con Orden ID : {dto.OrdenId}" } });
        }

    }
}