using Backend.features.DTOs;
using Backend.features.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.features.Services
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;
        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _service.GetAll();
            return productos is not null
            ? Ok(new { Results = productos })
            : NotFound(new
            {
                Results = new features.Results.ResultSuccess { successMessage = "No se encontrarón productos" }
            });
        }
        [HttpGet("{ProductoId}")]
        public async Task<IActionResult> GetById(int ProductoId)
        {
            var producto = await _service.GetById(ProductoId);
            return producto is not null
            ? Ok(new { Results = producto })
            : NotFound(new
            {
                Results = new features.Results.ResultError { errorMessage = $"No se encontró producto con el Producto ID : {ProductoId}" }
            });
        }

        [Authorize(Policy = "SoloAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoCreateDTO dto)
        {
            var productoCreado = await _service.Create(dto);
            if (productoCreado is null) return NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se encontró la categoria del producto con el Categoria ID : {dto.Categoria.Id}" } });
            return CreatedAtAction(nameof(GetById), new { Id = productoCreado.Id }, productoCreado);
        }

        [Authorize(Policy = "SoloAdmin")]
        [HttpPut("{ProductoId}")]
        public async Task<IActionResult> Update(int ProductoId, [FromBody] ProductoCreateDTO dto)
        {
            var creado = await _service.Update(ProductoId, dto);
            return creado
            ? Ok(new { Results = new features.Results.ResultSuccess { successMessage = "Producto actualizado con éxito." } })
            : NotFound(new { Results = new features.Results.ResultError { errorMessage = $"No se pudo actualizar el producto con el Producto Id : {ProductoId}" } });
        }
        
        [Authorize(Policy ="SoloAdmin")]
        [HttpDelete("{ProductoId}")]
        public async Task<IActionResult> DeleteByLogic(int ProductoId)
        {
            var eliminado = await _service.DeleteByLogic(ProductoId);
            return eliminado
            ? Ok(new { Results = new features.Results.ResultSuccess { successMessage = "Producto eliminado con éxito"} })
            : NotFound(new { Results = new features.Results.ResultError {errorMessage = $"No se pudo eliminar el producto con el Producto ID : {ProductoId}"}});
        }
    }
}