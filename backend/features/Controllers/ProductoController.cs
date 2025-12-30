using Backend.Features.DTOs;
using Backend.Features.Results;
using Backend.Features.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Controllers
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
        
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _service.GetAll();
            return Ok(new ApiResponse<List<ProductoReadDTO>>(productos));
        }

        // [HttpGet("actives")]
        // public async Task<IActionResult> GetAllActives()
        // {
        //     var productos = await _service.GetAllActives();
        //     return Ok(new ApiResponse<List<ProductoReadDTO>>(productos));
            
        // }
        
        [HttpGet("{ProductoId}")]
        public async Task<IActionResult> GetById(int ProductoId)
        {
            var producto = await _service.GetById(ProductoId);
            return Ok(new ApiResponse<ProductoReadDTO>(producto));
            
        }
    
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoCreateDTO dto)
        {
            var productoCreado = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { ProductoId = productoCreado.Id }, new {Results = productoCreado});
        }

        [Authorize]
        [HttpPut("{ProductoId}")]
        public async Task<IActionResult> Update(int ProductoId, [FromBody] ProductoUpdateDTO dto)
        {
            var actualizado = await _service.Update(ProductoId, dto);
            return Ok(ApiResponse<ResultSuccess>.Success("Producto actualizado con éxito."));
        }
        
        [Authorize]
        [HttpDelete("{ProductoId}")]
        public async Task<IActionResult> DeleteByLogic(int ProductoId)
        {
            var eliminado = await _service.DeleteByLogic(ProductoId);
            return Ok(ApiResponse<ResultSuccess>.Success("Producto eliminado con éxito."));        
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllPagerFilterAdmin([FromQuery] ProductoQuery query)
        {
            var productos = await _service.GetAllPagerFilterAdmin(query);
            return Ok(new ApiResponse<ResultadoPaginado<ProductoReadDTO>>(productos));
        }

        [HttpGet("actives")]
        public async Task<IActionResult> GetAllPagerFilterUser([FromQuery] ProductoQuery query)
        {
            var productos = await _service.GetAllPagerFilterUser(query);
            return Ok(new ApiResponse<ResultadoPaginado<ProductoReadDTO>>(productos));
        }
    }
}