
using Backend.Features.DTOs;
using Backend.Features.Results;
using Backend.Features.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;
        public CategoriaController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _service.GetAll();

            return Ok(new ApiResponse<List<CategoriaReadDTO>>(categorias));
        }
        
        [HttpGet("{CatId}")]
        public async Task<IActionResult> GetById(int CatId)
        {
            var categoria = await _service.GetById(CatId);
            return Ok(new ApiResponse<CategoriaReadDTO>(categoria));
        }

        [Authorize(Policy = "SoloAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoriaCreateDTO dto)
        {
            var cat = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { CatId = cat.Id }, new {Results = cat} );
        }
        
        [Authorize(Policy ="SoloAdmin")]
        [HttpPut("{CatId}")]
        public async Task<IActionResult> Update(int CatId, [FromBody] CategoriaCreateDTO dto)
        {
            var exito = await _service.Update(CatId, dto);
            return Ok(ApiResponse<ResultSuccess>.Success("Categoria actualizada con éxito"));
        }

    }
}