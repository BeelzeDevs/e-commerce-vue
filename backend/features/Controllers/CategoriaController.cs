
using Backend.features.DTOs;
using Backend.features.Models;
using Backend.features.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.features.Controllers
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
            
            return categorias is not null 
            ? Ok(new { Results = categorias })
            : NotFound(new { Results =  new features.Results.ResultError { errorMessage = "No se encontraron categorias" }
            });
        }
        [HttpGet("/{CatId}")]
        public async Task<IActionResult> GetById(int CatId)
        {
            var categoria = await _service.GetById(CatId);
            return categoria is not null
            ? Ok(new { Results = categoria })
            : NotFound(new
            {
                Results = new features.Results.ResultError { errorMessage = "No se encontró la categoria" }
            });
        }

        [Authorize(Policy = "SoloAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoriaCreateDTO dto)
        {
            var cat = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { Id = cat.Id }, cat);
        }
        
        [Authorize(Policy ="SoloAdmin")]
        [HttpPut("{CatId}")]
        public async Task<IActionResult> Update(int CatId, [FromBody] CategoriaCreateDTO dto)
        {
            var exito = await _service.Update(CatId, dto);
            return exito 
            ? Ok(new { Results = new features.Results.ResultSuccess{ successMessage = "Categoria actualizada con éxito"} }) 
            : NotFound( new { Results = new features.Results.ResultError { errorMessage = $"No se encontro la Categoria, datos incorrectos ID Categoria : {CatId}"} });
        }

    }
}