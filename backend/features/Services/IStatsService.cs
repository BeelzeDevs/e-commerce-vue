using Backend.Features.DTOs;

namespace Backend.Features.Services
{
    public interface IStatsService
    {
        Task<VentasTotalesDTO> GetVentasTotales();
        Task<List<VentasPorMesDTO>> GetVentasMensuales();
        Task<List<OrdenesPorEstadoDTO>> GetOrdenesPorEstados();
        Task<List<TopProductoDTO>> GetTopProductos(int top);
    }
}