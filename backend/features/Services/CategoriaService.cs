using Backend.Data;
using Backend.features.DTOs;
using Backend.features.Services;
using Microsoft.EntityFrameworkCore;
namespace Backend.features.Services
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
            return await _context.Categorias.Select(
                cat => new CategoriaReadDTO
                {
                    Id = cat.Id,
                    Nombre = cat.Nombre
                }
            ).ToListAsync();
        }
        public async Task<CategoriaReadDTO?> GetById(int id)
        {
            var cat = await _context.Categorias.FirstOrDefaultAsync(cat => cat.Id == id);

            if (cat is null) return null;

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


            _context.Categorias.Add(cat);
            await _context.SaveChangesAsync();

            return await GetById(cat.Id) ?? throw new Exception("Error al crear Categoria");
        }

        public async Task<bool> Update(int id, CategoriaCreateDTO dto)
        {
            var cat = await _context.Categorias.FindAsync(id);

            if (cat is null) return false;
            cat.Nombre = dto.Nombre;

            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}