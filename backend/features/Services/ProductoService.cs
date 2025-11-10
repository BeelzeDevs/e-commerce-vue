using Backend.Data;
using Backend.Features.DTOs;
using Backend.Features.Models;
using Backend.Features.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Features.Services
{
    public class ProductoService : IProductoService
    {
        private readonly EcommerceDbContext _context;
        public ProductoService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductoReadDTO>> GetAll()
        {
            return await _context.Productos.Include(p => p.Categoria)
            .Select(p => new ProductoReadDTO
            {
                Id = p.Id,
                Categoria = new CategoriaReadDTO
                {
                    Id = p.Categoria.Id,
                    Nombre = p.Categoria.Nombre
                },
                Nombre = p.Nombre,
                Marca = p.Marca,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                Imagen = p.Imagen,
                Estado = p.Estado
            }).ToListAsync();
        }
        public async Task<List<ProductoReadDTO>> GetAllActives()
        {
            return await _context.Productos.Include(p => p.Categoria).Where(p=> p.Estado == true)
            .Select(p => new ProductoReadDTO
            {
                Id = p.Id,
                Categoria = new CategoriaReadDTO
                {
                    Id = p.Categoria.Id,
                    Nombre = p.Categoria.Nombre
                },
                Nombre = p.Nombre,
                Marca = p.Marca,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                Imagen = p.Imagen,
                Estado = p.Estado
            }).ToListAsync();
        }
        public async Task<ProductoReadDTO> GetById(int id)
        {
            var prod = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (prod is null) throw new Exception($"No se encontró producto con el Producto ID: {id}");
            return new ProductoReadDTO
            {
                Id = prod.Id,
                Categoria = new CategoriaReadDTO
                {
                    Id = prod.Categoria.Id,
                    Nombre = prod.Categoria.Nombre
                },
                Nombre = prod.Nombre,
                Marca = prod.Marca,
                Descripcion = prod.Descripcion,
                Precio = prod.Precio,
                Stock = prod.Stock,
                Imagen = prod.Imagen,
                Estado = prod.Estado
            };
        }
        public async Task<ProductoReadDTO> Create(ProductoCreateDTO dto)
        {
            var catExiste = await _context.Categorias.FindAsync(dto.CategoriaId);
            if (catExiste is null) throw new Exception($"La categoria del producto a crear no existe, Categoria ID : {dto.CategoriaId}");

            var prod = new Features.Models.Producto
            {
                Nombre = dto.Nombre,
                CategoriaId = dto.CategoriaId,
                Marca = dto.Marca,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                Imagen = dto.Imagen,
                Estado = dto.Estado
            };

            _context.Productos.Add(prod);
            await _context.SaveChangesAsync();
            return await GetById(prod.Id) ?? throw new Exception($"Error al crear un producto, DTO {dto}");

        }
        public async Task<bool> Update(int id, ProductoUpdateDTO dto)
        {
            var prod = await _context.Productos.Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
            if (prod is null) return false;

            prod.Nombre = dto.Nombre;
            prod.CategoriaId = dto.CategoriaId;
            prod.Marca = dto.Marca;
            prod.Descripcion = dto.Descripcion;
            prod.Precio = dto.Precio;
            prod.Stock = dto.Stock;
            prod.Imagen = dto.Imagen;
            prod.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteByLogic(int id)
        {
            var prod = await _context.Productos.FindAsync(id);
            if (prod is null) return false;

            prod.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}