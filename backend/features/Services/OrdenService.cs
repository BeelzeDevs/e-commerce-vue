using Backend.Features.Services;
using Backend.Features.DTOs;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Backend.Features.Models;
using System.Security.Claims;

namespace Backend.Features.Services
{
    public class OrdenService : IOrdenService
    {
        private readonly EcommerceDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccesor;
        public OrdenService(EcommerceDbContext context, IHttpContextAccessor httpContextAccesor)
        {
            _context = context;
            _httpContextAccesor = httpContextAccesor;
        }
        private int GetClaimUsuarioId()
        {
            var claim = _httpContextAccesor.HttpContext?.User.FindFirst("UsuarioId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
        private string? GetRol()
        {
            return _httpContextAccesor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

        }
        private bool EsAdmin()
        {
            return GetRol() == "Administrador";
        }
        private bool EsMismoUsuario(int UsuarioId)
        {
            return GetClaimUsuarioId() == UsuarioId;
        }  
        public async Task<List<OrdenReadDTO>> GetAll()
        {
            return await _context.Ordenes.Include(e => e.Detalles)
            .Select(o => new OrdenReadDTO
            {
                Id = o.Id,
                UsuarioId = o.UsuarioId,
                Fecha = o.Fecha,
                Total = o.Total
            }).ToListAsync();
        }
        public async Task<OrdenReadDTO> GetByOrdenId(int id)
        {
            var orden = await _context.Ordenes.FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) throw new Exception($"La Orden no existe con el Orden ID : {id}");
            if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            
            return new OrdenReadDTO
            {
                Id = orden.Id,
                UsuarioId = orden.UsuarioId,
                Fecha = orden.Fecha,
                Total = orden.Total
            };
        }
        public async Task<List<OrdenReadDTO>> GetOrdenesByUsarioId(int UsuarioId)
        {
            var ordenes = await _context.Ordenes
            .Where(o => o.UsuarioId == UsuarioId)
            .Select(o => new OrdenReadDTO
            {
                Id = o.Id,
                UsuarioId = o.UsuarioId,
                Fecha = o.Fecha,
            })
            .ToListAsync();

            if (!EsMismoUsuario(UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            if (ordenes.Count == 0) 
                throw new Exception($"No se encontraron ordenes con el Usuario ID : {UsuarioId}");
            
            return ordenes;
            
        }
        public async Task<OrdenReadDTO> Create(OrdenCreateDTO dto)
        {
            var orden = new Backend.Features.Models.Orden
            {
                UsuarioId = dto.UsuarioId,
                Fecha = DateTime.UtcNow,
                Total = 0
            };
            if (orden is null) throw new Exception($"No se pudo crear la orden,datos incorrectos OrdenCreateDTO : {dto}");

            if (!EsMismoUsuario(dto.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();
            return new OrdenReadDTO
            {
                Id = orden.Id,
                UsuarioId = orden.UsuarioId,
                Fecha = orden.Fecha,
                Total = orden.Total
            };
        }

        public async Task<bool> Update(int id, OrdenUpdateDTO dto)
        {
            var orden = await _context.Ordenes.FindAsync(id);
            if (orden is null) throw new Exception($"Orden no encontrada con el orden ID : {id}");

            if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            
            orden.UsuarioId = dto.UsuarioId;
            orden.Total = dto.Total;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var orden = await _context.Ordenes.Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) throw new Exception($"Orden no encontrada con el orden ID: {id}");

            if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            var detalles = await _context.DetalleOrdenes.Where(detalle => detalle.OrdenId == id)
            .ToListAsync();
            if (detalles is not null) _context.RemoveRange(detalles);

            _context.Ordenes.Remove(orden);
            await _context.SaveChangesAsync();
            return true;


        }
        private async Task ActualizarTotalOrden(int OrdenId)
        {
            var orden = await _context.Ordenes.Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.Id == OrdenId);

            if (orden is not null)
            {
                orden.Total = orden.Detalles.Sum(d => d.Subtotal);
                await _context.SaveChangesAsync();
            }
        }
        
        // Detalles
        public async Task<List<DetalleReadDTO>> GetDetallesByOrdenId(int OrdenId)
        {
            var orden = await _context.Ordenes.FindAsync(OrdenId) ?? throw new Exception($"Orden no encontrada con el Orden ID : {OrdenId}");
            if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            var detalles = await _context.DetalleOrdenes
            .Include(dxo => dxo.Producto)
                .ThenInclude(p => p.Categoria)
            .Where(dxo => dxo.OrdenId == OrdenId)
            .Select(dxo => new DetalleReadDTO
            {
                OrdenId = dxo.OrdenId,
                Cantidad = dxo.Cantidad,
                Subtotal = dxo.Subtotal,
                Producto = new ProductoReadDTO
                {
                    Id = dxo.Producto.Id,
                    Categoria = new CategoriaReadDTO
                    {
                        Id = dxo.Producto.Categoria.Id,
                        Nombre = dxo.Producto.Categoria.Nombre
                    },
                    Nombre = dxo.Producto.Nombre,
                    Marca = dxo.Producto.Marca,
                    Descripcion = dxo.Producto.Descripcion,
                    Precio = dxo.Producto.Precio,
                    Stock = dxo.Producto.Stock,
                    Imagen = dxo.Producto.Imagen,
                    Estado = dxo.Producto.Estado
                }
            }).ToListAsync();
      

            return detalles is not null ? detalles : throw new Exception($"No se encontraron Detalles con el Orden ID : ${OrdenId}");

        }
        public async Task<bool> AddDetalle(DetalleCreateDTO dto)
        {
            var orden = await _context.Ordenes.FindAsync(dto.OrdenId);
            if (orden is null) throw new Exception($"Orden no encontrada con el Orden ID : {dto.OrdenId}");
            var producto = await _context.Productos.FindAsync(dto.ProductoId);
            if (producto is null) throw new Exception($"Producto no encontrado con el Producto ID : {dto.ProductoId}");
            var existe = await _context.DetalleOrdenes.FindAsync(dto.OrdenId,dto.ProductoId);
            if (existe is not null) throw new Exception($"Detalle ya existe el Detalle ID : {dto.OrdenId},{dto.ProductoId}");

            if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            var detalle = new DetalleOrden
            {
                OrdenId = dto.OrdenId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                Subtotal = dto.Cantidad * producto.Precio
            };

            _context.DetalleOrdenes.Add(detalle);
            await _context.SaveChangesAsync();

            await ActualizarTotalOrden(orden.Id);
            return true;
        }
        public async Task<bool> DeleteDetalle(DetalleDeleteDTO dto)
        {
            var detalle = await _context.DetalleOrdenes.Include(d=>d.Orden).FirstOrDefaultAsync(d=> d.ProductoId == dto.ProductoId && d.OrdenId == dto.OrdenId);
            if (detalle is null) throw new Exception($"Detalle no encontrado con el Detalle ID : {dto.OrdenId},{dto.ProductoId}");

            if (!EsMismoUsuario(detalle.Orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            _context.DetalleOrdenes.Remove(detalle);
            await _context.SaveChangesAsync();
            await ActualizarTotalOrden(detalle.OrdenId);
            return true;
        }
        public async Task<bool> UpdateDetalle(DetalleCreateDTO dto)
        {
            var detalle = await _context.DetalleOrdenes.Include(d=> d.Orden).FirstOrDefaultAsync(d=> d.OrdenId == dto.OrdenId && d.ProductoId == dto.ProductoId);
            if (detalle is null) throw new Exception($"Detalle no encontrado con el Detalle ID : {dto.OrdenId},{dto.ProductoId}");

            var producto = await _context.Productos.FindAsync(dto.ProductoId);
            if (producto is null) throw new Exception($"Producto no encontrado con el Producto ID : {dto.ProductoId}");

            if (!EsMismoUsuario(detalle.Orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso no authorizado");

            detalle.Cantidad = dto.Cantidad;
            detalle.Subtotal = dto.Cantidad * producto.Precio;
            await _context.SaveChangesAsync();
            await ActualizarTotalOrden(detalle.OrdenId);
            return true;
        }
    }    
}