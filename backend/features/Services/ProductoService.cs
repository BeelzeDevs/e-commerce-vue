using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpAccessor;
        public ProductoService(EcommerceDbContext context, IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _httpAccessor = httpAccessor;
        }
        private int getIdUsuario()
        {
            var claim = _httpAccessor.HttpContext?.User.FindFirst("UsuarioId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
        private string? getRol()
        {
            return _httpAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        }
        private bool EsAdmin() => getRol() == "Administrador";
        private bool EsMismoUsuario(int id)
        {
            return getIdUsuario() == id;
        }

        public async Task<List<ProductoReadDTO>> GetAll()
        {
            
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

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
            })
            .OrderBy(p => p.Id)
            .ToListAsync();
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
            })
            .OrderBy(p => p.Id)
            .ToListAsync();
        }
        public async Task<ProductoReadDTO> GetById(int id)
        {
            var prod = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (prod is null) throw new KeyNotFoundException($"No se encontró producto con el Producto ID: {id}");
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
            
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

            var catExiste = await _context.Categorias.FindAsync(dto.CategoriaId);
            if (catExiste is null) throw new KeyNotFoundException($"La categoria del producto a crear no existe, Categoria ID : {dto.CategoriaId}");

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
            return await GetById(prod.Id) ?? throw new ArgumentException($"Error al crear un producto, DTO {dto}");

        }
        public async Task<bool> Update(int id, ProductoUpdateDTO dto)
        {
            
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

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
            
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

            var prod = await _context.Productos.FindAsync(id);
            if (prod is null) return false;

            prod.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ResultadoPaginado<ProductoReadDTO>> GetAllPagerFilterAdmin(ProductoQuery prodQuery)
        {
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no Autorizado");

            var query =  _context.Productos.AsNoTracking().AsQueryable();

            // Filtros
            if(prodQuery.Estado.HasValue && prodQuery.Estado is not null) 
                query = query.Where(p=> p.Estado == prodQuery.Estado);
            
            if (!string.IsNullOrEmpty(prodQuery.Search))
            {
                var search = prodQuery.Search.ToLower();
                query = query.Where(p=> 
                    p.Nombre.ToLower().Contains(search) ||
                    p.Descripcion.ToLower().Contains(search) ||
                    p.Marca.ToLower().Contains(search)
                );
            }
            
            if(prodQuery.CategoriaId.HasValue)
                query = query.Where(p=> p.CategoriaId == prodQuery.CategoriaId);
            
            //
            var totalItems = await query.CountAsync();
            var totalPages = 0;
            if( totalItems % prodQuery.PageSize == 0) totalPages = totalItems / prodQuery.PageSize;
            else totalPages = totalItems / prodQuery.PageSize + 1 ;

            var items = await query.OrderBy(p=> p.Id)
                        .Skip( (prodQuery.Page - 1) * prodQuery.PageSize )
                        .Take(prodQuery.PageSize)
                        .Select(p=> new ProductoReadDTO
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Descripcion = p.Descripcion,
                            Imagen = p.Imagen,
                            Marca = p.Marca,
                            Categoria = new CategoriaReadDTO
                            {
                                Id = p.CategoriaId,
                                Nombre = p.Categoria.Nombre,
                            },
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Estado = p.Estado,
                        })
                        .ToListAsync();
            return new ResultadoPaginado<ProductoReadDTO>
            {
                Items = items,
                TotalItems = totalItems,
                Page = prodQuery.Page,
                PageSize = prodQuery.PageSize,
                TotalPages = totalPages,

            };
        }
        public async Task<ResultadoPaginado<ProductoReadDTO>> GetAllPagerFilterUser(ProductoQuery prodQuery)
        {

            var query =  _context.Productos.AsNoTracking().AsQueryable();

            // Filtros
            query = query.Where(p=> p.Estado == true);

            if (!string.IsNullOrEmpty(prodQuery.Search))
            {
                var search = prodQuery.Search.ToLower();
                query = query.Where(p=> 
                    p.Nombre.ToLower().Contains(search) ||
                    p.Descripcion.ToLower().Contains(search) ||
                    p.Marca.ToLower().Contains(search)
                );
            }
            
            if(prodQuery.CategoriaId.HasValue)
                query = query.Where(p=> p.CategoriaId == prodQuery.CategoriaId);
            
            //
            var totalItems = await query.CountAsync();
            var totalPages = 0;
            if( totalItems % prodQuery.PageSize == 0) totalPages = totalItems / prodQuery.PageSize;
            else totalPages = totalItems / prodQuery.PageSize + 1 ;

            var items = await query.OrderBy(p=> p.Id)
                        .Skip( (prodQuery.Page - 1) * prodQuery.PageSize )
                        .Take(prodQuery.PageSize)
                        .Select(p=> new ProductoReadDTO
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Descripcion = p.Descripcion,
                            Imagen = p.Imagen,
                            Marca = p.Marca,
                            Categoria = new CategoriaReadDTO
                            {
                                Id = p.CategoriaId,
                                Nombre = p.Categoria.Nombre,
                            },
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Estado = p.Estado,
                        })
                        .ToListAsync();
            return new ResultadoPaginado<ProductoReadDTO>
            {
                Items = items,
                TotalItems = totalItems,
                Page = prodQuery.Page,
                PageSize = prodQuery.PageSize,
                TotalPages = totalPages,

            };
        }
    }
}