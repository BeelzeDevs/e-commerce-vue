using Backend.Data;
using Backend.Features.DTOs;
using Backend.Features.Services;
using Microsoft.EntityFrameworkCore;
namespace Backend.Features.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly EcommerceDbContext _context;

        public CategoriaService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaReadDTO>> GetAll()
        {
            return await _context.Categorias
                .AsNoTracking() // No se van a modificar por lo que NoTracking mejora el rendimiento
                .Select(
                cat => new CategoriaReadDTO
                {
                    Id = cat.Id,
                    Nombre = cat.Nombre
                }
            ).ToListAsync();
        }
        public async Task<CategoriaReadDTO> GetById(int id)
        {
            var cat = await _context.Categorias.FirstOrDefaultAsync(cat => cat.Id == id);

            if (cat is null) throw new Exception($"Categoria no encontrada con el Categoria ID : {id}");

            return new CategoriaReadDTO
            {
                Id = cat.Id,
                Nombre = cat.Nombre
            };
        }

        public async Task<CategoriaReadDTO> Create(CategoriaCreateDTO dto)
        {
            var cat = new Models.Categoria
            {
                Nombre = dto.Nombre
            };
            if (cat is null) throw new Exception($"No se pudo crear la categoria, DTO : {dto}");

            _context.Categorias.Add(cat);
            await _context.SaveChangesAsync();

            return new CategoriaReadDTO
                {
                    Id = cat.Id,
                    Nombre = cat.Nombre
                };
        }

        public async Task<bool> Update(int id, CategoriaCreateDTO dto)
        {
            var cat = await _context.Categorias.FindAsync(id);

            if (cat is null) throw new Exception($"No se pudo encontrar la categoria a actualizar, con el Categoria ID: {id}");
            cat.Nombre = dto.Nombre;

            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}