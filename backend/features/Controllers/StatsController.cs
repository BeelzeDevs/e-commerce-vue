using Microsoft.AspNetCore.Mvc;
using Backend.Features.Services;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.HttpResults;
using Backend.Features.Results;
using Backend.Features.DTOs;
namespace Backend.Features.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Policy ="SoloAdmin")]

    public class StatsController : ControllerBase
    {
        public readonly IStatsService _service;
        public StatsController(IStatsService service)
        {
            _service = service;
        }
        
        [Authorize]
        [HttpGet("ventas-totales")]
        public async Task<IActionResult> GetVentasTotales()
        {
            var ventasTotales = await _service.GetVentasTotales();
            return Ok(new ApiResponse<VentasTotalesDTO>(ventasTotales));
        }
        
        [Authorize]
        [HttpGet("ventas-mensuales")]
        public async Task<IActionResult> GetVentasMensuales()
        {
            var ventasMensuales = await _service.GetVentasMensuales();
            return Ok(new ApiResponse<List<VentasPorMesDTO>>(ventasMensuales));
        }
        
        [Authorize]
        [HttpGet("ordenes-por-estado")]
        public async Task<IActionResult> GetOrdenesPorEstado()
        {
            var ventasPorEstado = await _service.GetOrdenesPorEstados();
            return Ok(new ApiResponse<List<OrdenesPorEstadoDTO>>(ventasPorEstado));
        }

        // /api/stats/top-productos?top=5
        
        [Authorize]
        [HttpGet("top-productos")]
        public async Task<IActionResult> GetTopProductos([FromQuery]int top = 5)
        {
            var topProductos = await _service.GetTopProductos(top);
            return Ok(new ApiResponse<List<TopProductoDTO>>(topProductos));
        }
    }


}