using Backend.features.Services;
using Backend.features.DTOs;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Backend.features.Models;

namespace Backend.features.Services
{
    public class OrdenService : IOrdenService
    {
        private readonly EcommerceDbContext _context;
        public OrdenService(EcommerceDbContext context)
        {
            _context = context;
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
        public async Task<OrdenReadDTO?> GetById(int id)
        {
            var orden = await _context.Ordenes.FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) return null;
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

            return ordenes is not null ? ordenes : throw new Exception($"No se encontraron ordenes con el Usuario ID : {UsuarioId}");
            
        }
        public async Task<OrdenReadDTO?> Create(OrdenCreateDTO dto)
        {
            var orden = new Backend.features.Models.Orden
            {
                UsuarioId = dto.UsuarioId,
                Fecha = DateTime.Now,
                Total = dto.Total
            };
            if (orden is null) return null;

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

        public async Task<bool> Update(int id, OrdenCreateDTO dto)
        {
            var orden = await _context.Ordenes.FindAsync(id);
            if (orden is null) return false;

            orden.UsuarioId = dto.UsuarioId;
            orden.Total = dto.Total;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var orden = await _context.Ordenes.Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) return false;

            var detalles = await _context.DetalleOrdenes.Where(detalle => detalle.OrdenId == id)
            .ToListAsync();
            if (detalles is not null) _context.RemoveRange(detalles);

            _context.Ordenes.Remove(orden);
            await _context.SaveChangesAsync();
            return true;


        }
        
        public async Task<List<DetalleReadDTO>> GetDetallesByOrdenId(int OrdenId)
        {


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
            if (orden is null) return false;
            var producto = await _context.Productos.FindAsync(dto.ProductoId);
            if (producto is null) return false;
            var existe = await _context.DetalleOrdenes.FindAsync(dto.OrdenId,dto.ProductoId);
            if (existe is not null) return false;

            var detalle = new DetalleOrden
            {
                OrdenId = dto.OrdenId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                Subtotal = dto.Cantidad * producto.Precio
            };

            _context.DetalleOrdenes.Add(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteDetalle(DetalleDeleteDTO dto)
        {
            var orden = await _context.DetalleOrdenes.FindAsync(dto.OrdenId, dto.ProductoId);
            if (orden is null) return false;

            var detalle = new DetalleOrden
            {
                OrdenId = dto.OrdenId,
                ProductoId = dto.ProductoId
            };

            _context.DetalleOrdenes.Remove(detalle);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateDetalle(DetalleCreateDTO dto)
        {
            var detalle = await _context.DetalleOrdenes.FindAsync(dto.OrdenId, dto.ProductoId);
            if (detalle is null) return false;

            var producto = await _context.Productos.FindAsync(dto.ProductoId);
            if (producto is null) return false;

            detalle.Cantidad = dto.Cantidad;
            detalle.Subtotal = dto.Cantidad * producto.Precio;
            return true;
        }
    }    
}