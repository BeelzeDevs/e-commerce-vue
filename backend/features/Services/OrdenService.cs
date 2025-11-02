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
    }    
}