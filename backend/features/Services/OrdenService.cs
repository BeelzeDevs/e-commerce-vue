using Backend.Features.Services;
using Backend.Features.DTOs;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Backend.Features.Models;
using System.Security.Claims;
using System.Text;

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
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no autorizado");
            return await _context.Ordenes.Include(e => e.Detalles)
            .Include(o => o.Usuario)
            .Select(o => new OrdenReadDTO
            {
                Id = o.Id,
                Usuario = new UsuarioReadDTO
                {
                  Id = o.Usuario.Id,
                  Nombre = o.Usuario.Nombre,
                  Email = o.Usuario.Email,
                  Rol = new RolReadDTO
                  {
                      Id = o.Usuario.RolId,
                      Nombre = o.Usuario.Rol.Nombre,
                  },
                  FechaRegistro = o.Usuario.FechaRegistro,
                  Estado = o.Usuario.Estado,
                },
                Fecha = o.Fecha,
                Total = o.Total,
                Estado = o.Estado
            })
            .OrderBy(o => o.Id)
            .ToListAsync();
        }
        public async Task<ResultadoPaginado<OrdenReadDTO>> GetAllPagerFilterAdmin(OrdenQueryDTO queryOrd)
        {
            var query  = _context.Ordenes.Include(o=> o.Usuario).ThenInclude(u=>u.Rol).Include(o=>o.Detalles).AsNoTracking().AsQueryable();

            if(!string.IsNullOrEmpty(queryOrd.Estado)) query = query.Where(o=> o.Estado.ToLower().Contains(queryOrd.Estado.ToLower()));
            if(!string.IsNullOrEmpty(queryOrd.SearchUsuario)) {
                query = query.Where(o=> o.Usuario.Nombre.ToLower().Contains(queryOrd.SearchUsuario.ToLower()) ||
                o.Usuario.Email.ToLower().Contains(queryOrd.SearchUsuario.ToLower()) );
            }
            if (queryOrd.Fecha.HasValue)
            {
              var fechaLocal = queryOrd.Fecha.Value.Date;
              var fechaUTC = DateTime.SpecifyKind(fechaLocal, DateTimeKind.Utc);
              var fechaFin = fechaUTC.AddDays(1);
              query = query.Where(o=> o.Fecha >= fechaUTC && o.Fecha < fechaFin);   
            }
            if(queryOrd.RolId != null) query = query.Where(o=> o.Usuario.RolId == queryOrd.RolId);

            var totalOrdenes = await query.CountAsync();
            var totalPages = 0;
            if(totalOrdenes % queryOrd.PageSize == 0) totalPages = totalOrdenes / queryOrd.PageSize;
            else totalPages = totalOrdenes / queryOrd.PageSize + 1;

            var ordenes = await query.OrderBy(o=> o.UsuarioId)
                        .Skip( (queryOrd.Page - 1) * queryOrd.PageSize)
                        .Take(queryOrd.PageSize).Select(o=> new OrdenReadDTO
                        {
                            Id = o.Id,
                            Usuario = new UsuarioReadDTO
                            {
                              Id = o.Usuario.Id,
                              Rol = new RolReadDTO
                              {
                                    Id = o.Usuario.RolId,
                                    Nombre = o.Usuario.Rol.Nombre,   
                              },
                              Nombre = o.Usuario.Nombre,
                              Email = o.Usuario.Email,
                              FechaRegistro = o.Usuario.FechaRegistro,
                              Estado = o.Usuario.Estado,
                            },
                            Fecha = o.Fecha,
                            Total = o.Total,
                            Estado = o.Estado,
                            
                        }).ToListAsync();

            return new ResultadoPaginado<OrdenReadDTO>
            {
                Items = ordenes,
                TotalItems = totalOrdenes,
                TotalPages = totalPages,
                PageSize = queryOrd.PageSize,
                Page = queryOrd.Page,
            };
        }

        public async Task<OrdenReadDTO> GetByOrdenId(int id)
        {
            var orden = await _context.Ordenes
            .Include(o => o.Usuario)
            .FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) throw new KeyNotFoundException($"La Orden no existe con el Orden ID : {id}");
            if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            
            return new OrdenReadDTO
            {
                Id = orden.Id,
                Usuario = new UsuarioReadDTO
                {
                  Id = orden.Usuario.Id,
                  Nombre = orden.Usuario.Nombre,
                  Email = orden.Usuario.Email,
                  Rol = new RolReadDTO
                  {
                      Id = orden.Usuario.RolId,
                      Nombre = orden.Usuario.Rol.Nombre,
                  },
                  FechaRegistro = orden.Usuario.FechaRegistro,
                  Estado = orden.Usuario.Estado,

                },
                Fecha = orden.Fecha,
                Total = orden.Total,
                Estado = orden.Estado,
            };
        }
        public async Task<List<OrdenReadDTO>> GetOrdenesByUsarioId(int UsuarioId)
        {
            
            if (!EsMismoUsuario(UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            var ordenes = await _context.Ordenes
            .Include(o => o.Usuario)
            .Where(o => o.UsuarioId == UsuarioId)
            .Select(o => new OrdenReadDTO
            {
                Id = o.Id,
                Usuario = new UsuarioReadDTO
                {
                  Id = o.Usuario.Id,
                  Nombre = o.Usuario.Nombre,
                  Email = o.Usuario.Email,
                  Rol = new RolReadDTO
                  {
                      Id = o.Usuario.RolId,
                      Nombre = o.Usuario.Rol.Nombre,
                  },
                  FechaRegistro = o.Usuario.FechaRegistro,
                  Estado = o.Usuario.Estado,

                },
                Fecha = o.Fecha,
                Total = o.Total,
                Estado = o.Estado,
            })
            .ToListAsync();


            if (ordenes.Count == 0) 
                throw new InvalidOperationException($"No se encontraron ordenes con el Usuario ID : {UsuarioId}");
            
            return ordenes;
            
        }
        public bool CheckearStock(List<CarritoItem> carrito, List<Producto> productos)
        {
            foreach(var item in carrito)
            {
                var ProductoActual = productos.First(x=> x.Id == item.ProductoId);
                if(ProductoActual == null || ProductoActual.Stock < item.Cantidad)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<OrdenReadDTO> Create(OrdenCreateDTO dto)
        {
            var idUsuario = GetClaimUsuarioId();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x=> x.Id == idUsuario);
            if (usuario is null && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            var productosIds = dto.CarritoItems.Select(x=> x.ProductoId).ToList();
            var productos = await _context.Productos.Where(p=> productosIds.Contains(p.Id)).ToListAsync();

            var StockOk = CheckearStock(dto.CarritoItems, productos);

            if(!StockOk) throw new InvalidOperationException("Stock insuficiente para uno o más productos");
            
            // Preparo el total y los detalles
            decimal totalCompra = 0;
            var detalles = new List<DetalleOrden>();

            foreach(var item in dto.CarritoItems)
            {
                var ProductoActual = productos.First(x=> x.Id == item.ProductoId);
                var subtotal = ProductoActual.Precio * item.Cantidad;

                var detalle = new DetalleOrden
                {
                    ProductoId = ProductoActual.Id,
                    Precio_Producto = ProductoActual.Precio,
                    Cantidad = item.Cantidad,
                    Subtotal = subtotal,
                };
                detalles.Add(detalle);
                
                totalCompra +=  subtotal;
                ProductoActual.Stock -= item.Cantidad;

            }
            var orden = new Backend.Features.Models.Orden
            {
              UsuarioId = usuario!.Id,
              Total = totalCompra,
              Fecha = DateTime.UtcNow,
              Estado = "Pendiente", 
              Detalles = detalles, 
            };
            
            
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Ordenes.Add(orden);
                await _context.SaveChangesAsync();            

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            
            await _context.Entry(orden)
            .Reference(o => o.Usuario)
            .LoadAsync();

            await _context.Entry(orden.Usuario)
            .Reference(u => u.Rol)
            .LoadAsync();
            
            return new OrdenReadDTO {
                Id = orden.Id,
                Usuario = new UsuarioReadDTO
                {
                    Id = orden.UsuarioId,
                    Rol = new RolReadDTO
                    {
                        Id = orden.Usuario.RolId,
                        Nombre = orden.Usuario.Rol.Nombre,
                    },
                    Nombre = orden.Usuario.Nombre,
                    Email = orden.Usuario.Email,
                    FechaRegistro = orden.Usuario.FechaRegistro,
                    Estado = orden.Usuario.Estado,
                },
                Fecha = orden.Fecha,
                Estado = orden.Estado,
                Total = orden.Total,
            };
        }

        // public async Task<bool> Update(int id, OrdenUpdateDTO dto)
        // {
        //     var orden = await _context.Ordenes.FindAsync(id);
        //     if (orden is null) throw new KeyNotFoundException($"Orden no encontrada con el orden ID : {id}");

        //     if (!EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            
        //     orden.Fecha = dto.Fecha;
        //     orden.Estado = dto.Estado;
        //     orden.Total = dto.Total;

        //     await _context.SaveChangesAsync();
        //     return true;
        // }
        // public async Task<bool> Delete(int id)
        // {
        //     var orden = await _context.Ordenes.Include(o => o.Detalles)
        //     .FirstOrDefaultAsync(o => o.Id == id);

        //     if (orden is null) throw new KeyNotFoundException($"Orden no encontrada con el orden ID: {id}");

        //     if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

        //     var detalles = await _context.DetalleOrdenes.Where(detalle => detalle.OrdenId == id)
        //     .ToListAsync();
        //     if (detalles is not null) _context.RemoveRange(detalles);

        //     _context.Ordenes.Remove(orden);
        //     await _context.SaveChangesAsync();
        //     return true;


        // }
        // private async Task ActualizarTotalOrden(int OrdenId)
        // {
        //     var orden = await _context.Ordenes.Include(o => o.Detalles)
        //     .FirstOrDefaultAsync(o => o.Id == OrdenId);

        //     if (orden is not null)
        //     {
        //         orden.Total = orden.Detalles.Sum(d => d.Subtotal);
        //         await _context.SaveChangesAsync();
        //     }
        // }
        
        // Detalles
        public async Task<List<DetalleReadDTO>> GetDetallesByOrdenId(int OrdenId)
        {

            var orden = await _context.Ordenes.FindAsync(OrdenId) ?? throw new KeyNotFoundException($"Orden no encontrada con el Orden ID : {OrdenId}");
            if (!EsMismoUsuario(orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            var detalles = await _context.DetalleOrdenes
            .Include(dxo => dxo.Producto)
                .ThenInclude(p => p.Categoria)
            .Where(dxo => dxo.OrdenId == OrdenId)
            .Select(dxo => new DetalleReadDTO
            {
                OrdenId = dxo.OrdenId,
                Cantidad = dxo.Cantidad,
                Precio_Producto = dxo.Precio_Producto,
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
      

            return detalles is not null ? detalles : throw new KeyNotFoundException($"No se encontraron Detalles con el Orden ID : ${OrdenId}");

        }
        // public async Task<bool> DeleteDetalle(DetalleDeleteDTO dto)
        // {

        //     var detalle = await _context.DetalleOrdenes.Include(d=>d.Orden).FirstOrDefaultAsync(d=> d.ProductoId == dto.ProductoId && d.OrdenId == dto.OrdenId);
        //     if (detalle is null) throw new KeyNotFoundException($"Detalle no encontrado con el Detalle ID : {dto.OrdenId},{dto.ProductoId}");

        //     if (!EsMismoUsuario(detalle.Orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

        //     _context.DetalleOrdenes.Remove(detalle);
        //     await _context.SaveChangesAsync();
        //     await ActualizarTotalOrden(detalle.OrdenId);
        //     return true;
        // }
        // public async Task<bool> UpdateDetalle(DetalleCreateDTO dto)
        // {

        //     var detalle = await _context.DetalleOrdenes.Include(d=> d.Orden).FirstOrDefaultAsync(d=> d.OrdenId == dto.OrdenId && d.ProductoId == dto.ProductoId);
        //     if (detalle is null) throw new KeyNotFoundException($"Detalle no encontrado con el Detalle ID : {dto.OrdenId},{dto.ProductoId}");

        //     var producto = await _context.Productos.FindAsync(dto.ProductoId);
        //     if (producto is null) throw new KeyNotFoundException($"Producto no encontrado con el Producto ID : {dto.ProductoId}");

        //     if (!EsMismoUsuario(detalle.Orden.UsuarioId) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso no authorizado");

        //     detalle.Precio_Producto = producto.Precio;
        //     detalle.Cantidad = dto.Cantidad;
        //     detalle.Subtotal = dto.Cantidad * producto.Precio;
        //     await _context.SaveChangesAsync();
        //     await ActualizarTotalOrden(detalle.OrdenId);
        //     return true;
        // }
    }    
}