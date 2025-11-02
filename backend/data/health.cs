using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class Health : ControllerBase
    {
        private readonly EcommerceDbContext _context;
        public Health(EcommerceDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IResult> getHealth()
        {
            try
            {
                var connection = await _context.Database.CanConnectAsync();
                var roles = await _context.Roles.CountAsync();
                var usuarios = await _context.Usuarios.CountAsync();
                var productos = await _context.Productos.CountAsync();
                var categorias = await _context.Categorias.CountAsync();
                var ordenes = await _context.Ordenes.CountAsync();
                var detalleOrdenes = await _context.DetalleOrdenes.CountAsync();
                
                return Results.Ok(new
                {
                    conexión = connection,
                    Roles = $"Conexión tabla Roles Exitosa, cantidad roles actuales: {roles} ",
                    Usuarios = $"Conexión tabla Usuarios Exitosa, cantidad usuarios encontrados {usuarios}",
                    Productos = $"Conexión tabla Productos Exitosa, cantidad de productos encontrados {productos}",
                    Categorias = $"Conexión tabla Productos Exitosa, cantidad de categorias encontradas {categorias}",
                    Ordenes = $"Conexión tabla Productos Exitosa, cantidad de ordenes encontradas {ordenes}",
                    DetalleOrdenes = $"Conexión tabla Productos Exitosa, cantidad de detalle ordenes encontradas {detalleOrdenes}"

                });
            }catch(Exception err)
            {
                return Results.Problem($"Error conexión = {err.Message}");
            }
        }
    }
}