using System.Security.Claims;
using Backend.Data;
using Backend.Features.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Services
{
    public class StatsService : IStatsService
    {
        private readonly EcommerceDbContext _context;
        private readonly IHttpContextAccessor _httpAccessor;
        public StatsService(EcommerceDbContext context, IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _httpAccessor = httpAccessor;
        }
        private string? getRol()
        {
            return _httpAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        }
        private bool EsAdmin() => getRol() == "Administrador";
        public async Task<VentasTotalesDTO> GetVentasTotales()
        {
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

            var totalVentas = await _context.Ordenes.Where(o=> o.Estado == "Pagado" || o.Estado == "Enviado").SumAsync(o=> o.Total);
            var cantidadOrdenes = await _context.Ordenes.CountAsync(o=> o.Estado == "Pagado" || o.Estado == "Enviado");
            
            return new VentasTotalesDTO
            {
                VentasTotales = totalVentas,
                CantidadVentas = cantidadOrdenes,
                TicketPromedio = cantidadOrdenes == 0 ? 0 : (decimal) totalVentas /cantidadOrdenes  
            };

        }
        public async Task<List<VentasPorMesDTO>> GetVentasMensuales()
        {
            
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

            var totalMensual = await _context.Ordenes
            .Where(o=> o.Estado == "Pagado" || o.Estado == "Enviado")
            .GroupBy(o=> new {o.Fecha.Year, o.Fecha.Month})
            .Select(o=> new VentasPorMesDTO
            {
                Año = o.Key.Year,
                Mes = o.Key.Month,
                TotalVentas = o.Sum(o=> o.Total),
                CantidadOrdenes = o.Count()
            })
            .OrderBy(o=> o.Año)
            .ThenBy(o=> o.Mes)
            .ToListAsync();

            return totalMensual;
        }

        public async Task<List<OrdenesPorEstadoDTO>> GetOrdenesPorEstados()
        {
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

            return await _context.Ordenes
            .GroupBy(o=> o.Estado)
            .Select(o=> new OrdenesPorEstadoDTO
            {
                Estado = o.Key,
                Cantidad = o.Count()
            })
            .OrderByDescending(o=> o.Cantidad)
            .ToListAsync();
        }
        // top 5 si no se manda cantidad
        public async Task<List<TopProductoDTO>> GetTopProductos(int top = 5)
        {
            
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");
            
            return await _context.DetalleOrdenes
            .Where(o=> o.Orden.Estado == "Pagago" || o.Orden.Estado == "Enviado")
            .GroupBy(d => new {d.ProductoId , d.Producto.Nombre})
            .Select(d=> new TopProductoDTO
            {
                ProductoId = d.Key.ProductoId,
                Nombre = d.Key.Nombre,
                CantidadVendida = d.Sum(p=> p.Cantidad),
                TotalFacturado = d.Sum(p=> p.Cantidad * p.Precio_Producto)
            })
            .OrderByDescending(x => x.CantidadVendida)
            .Take(top)
            .ToListAsync();
        }
    }
}