using System.Security.Claims;
using System.Text.RegularExpressions;
using Backend.Data;
using Backend.Features.DTOs;
using Backend.Features.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Services
{
    public enum RolesEnum {Admin = 1 , Cliente = 2}
    public class UsuarioService : IUsuarioService
    {
        private readonly EcommerceDbContext _context;
        private readonly IHttpContextAccessor _httpAccessor;
        public UsuarioService(EcommerceDbContext context, IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _httpAccessor = httpAccessor;
        }
        private int GetClaimUsuarioId()
        {
            var claim = _httpAccessor.HttpContext?.User.FindFirst("UsuarioId")?.Value;
            return int.TryParse(claim, out int resultado) ? resultado : 0;
        }
        private string? GetRol()
        {
            var claim = _httpAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            return claim;
        }
        private bool EsAdmin() => GetRol() == "Administrador";
        private bool EsMismoUsuario(int UsuarioId) => UsuarioId == GetClaimUsuarioId();

        public async Task<List<UsuarioReadDTO>> GetAll()
        {
            
            if (!EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            return await _context.Usuarios.Include(u => u.Rol)
            .Select(u =>
                new UsuarioReadDTO
                {
                    Id = u.Id,
                    Rol = new RolReadDTO
                    {
                        Id = u.Rol!.Id,
                        Nombre = u.Rol!.Nombre
                    },
                    Nombre = u.Nombre,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro,
                    Estado = u.Estado
                }
            )
            .OrderBy(u=> u.Id)
            .ToListAsync();
        }

        public async Task<ResultadoPaginado<UsuarioReadDTO>> GetAllPagerFilterAdmin(UsuarioQueryDTO queryUser)
        {
            if(!EsAdmin()) throw new UnauthorizedAccessException("Acceso no autorizado");

            var query = _context.Usuarios.AsNoTracking().AsQueryable();

            // filtros
            if(queryUser.Estado.HasValue) query = query.Where(u=> u.Estado == queryUser.Estado);
            if (!string.IsNullOrEmpty(queryUser.Search)){
                query = query.Where(u=> 
                u.Email.ToLower().Contains(queryUser.Search.ToLower()) || u.Nombre.ToLower().Contains(queryUser.Search.ToLower())
                );
                
            }
            if(queryUser.Fecha.HasValue){ 
                var fechaLocal = queryUser.Fecha.Value.Date;
                var fechaUTC = DateTime.SpecifyKind(fechaLocal, DateTimeKind.Utc);

                var fechaFin = fechaUTC.AddDays(1);

                query = query.Where(u=> u.FechaRegistro >= fechaUTC && u.FechaRegistro < fechaFin );
            
            }
            if(queryUser.RolId.HasValue) query = query.Where(u=> u.RolId == queryUser.RolId);   

            var totalItems = await query.CountAsync();
            var totalPages = 0;
            if(totalItems % queryUser.PageSize == 0) totalPages = totalItems / queryUser.PageSize;
            else totalPages = totalItems / queryUser.PageSize + 1;

            var usuarios = await query.OrderBy(u=> u.Id)
            .Skip( (queryUser.Page - 1) * queryUser.PageSize)
            .Take(queryUser.PageSize)
            .Select(u=> new UsuarioReadDTO
            {
                Id = u.Id,
                Rol = new RolReadDTO
                {
                    Id = u.RolId,
                    Nombre = u.Rol.Nombre,
                },
                Email = u.Email,
                Nombre = u.Nombre,
                FechaRegistro = u.FechaRegistro,
                Estado = u.Estado,

            }).ToListAsync();

            return new ResultadoPaginado<UsuarioReadDTO>
            {
                Items = usuarios,
                Page = queryUser.Page,
                PageSize = queryUser.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,  
            };
        }

        public async Task<UsuarioReadDTO> GetById(int id)
        {
            if (!EsMismoUsuario(id) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null) throw new ArgumentException($"Usuario no encontrado con el Usuario ID : {id}");
            return new UsuarioReadDTO
            {
                Id = usuario.Id,
                Rol = new RolReadDTO
                {
                    Id = usuario.Rol.Id,
                    Nombre = usuario.Rol.Nombre
                },
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                Estado = usuario.Estado
            };
        }

        public async Task<UsuarioReadDTO> Create(UsuarioCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email requerido");

            if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("Nombre requerido");

            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Contraseña requerida");
                
            
            var regPassword = new Regex("^(?=.*[A-Z])[A-Za-zñÑ0-9]{6,20}$");
            var regNombre = new Regex("^[A-Za-z ]{5,60}$");
            var regEmail = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");

            if(!regPassword.IsMatch(dto.Password)) throw new ArgumentException("Contraseña de usuario a crear no cumple requisitos");
            if(!regNombre.IsMatch(dto.Nombre)) throw new ArgumentException("Nombre de usuario a crear no cumple requisitos");
            var emailNormalizado = dto.Email.Trim().ToLower();
            if(!regEmail.IsMatch(emailNormalizado)) throw new ArgumentException("Email de usuario a crear no cumple requisitos");
            var usuarioExiste = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Trim().ToLower().Equals(emailNormalizado));
            
            if(usuarioExiste != null ) throw new InvalidOperationException("Usuario ya existe");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var usuario = new Models.Usuario
            {
                RolId = (int)RolesEnum.Cliente,
                Nombre = dto.Nombre,
                Email = emailNormalizado,
                PasswordHash = hashedPassword,
                FechaRegistro = DateTime.UtcNow,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            
            await _context.Entry(usuario).Reference(u=>u.Rol).LoadAsync();

            return new UsuarioReadDTO
            {
                Id = usuario.Id,
                Rol = new RolReadDTO
                {
                    Id = usuario.Rol.Id,
                    Nombre = usuario.Rol.Nombre
                },
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                Estado = usuario.Estado
            };
        }
        public async Task<UsuarioReadDTO> CreateAdmin(CreateAdminDTO dto)
        {
            
            if (!EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email requerido");

            if (string.IsNullOrWhiteSpace(dto.Nombre)) throw new ArgumentException("Nombre requerido");

            if (string.IsNullOrWhiteSpace(dto.Password)) throw new ArgumentException("Contraseña requerida");
                
            
            var regPassword = new Regex("^(?=.*[A-Z])[A-Za-zñÑ0-9]{6,20}$");
            var regNombre = new Regex("^[A-Za-z ]{5,60}$");
            var regEmail = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");

            if(!regPassword.IsMatch(dto.Password)) throw new ArgumentException("Contraseña de usuario a crear no cumple requisitos");
            if(!regNombre.IsMatch(dto.Nombre)) throw new ArgumentException("Nombre de usuario a crear no cumple requisitos");
            var emailNormalizado = dto.Email.Trim().ToLower();
            if(!regEmail.IsMatch(emailNormalizado)) throw new ArgumentException("Email de usuario a crear no cumple requisitos");
            var usuarioExiste = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailNormalizado);
            
            if(usuarioExiste != null ) throw new InvalidOperationException("Usuario ya existe");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var usuario = new Models.Usuario
            {
                RolId = (int)RolesEnum.Admin,
                Nombre = dto.Nombre,
                Email = emailNormalizado,
                PasswordHash = hashedPassword,
                FechaRegistro = DateTime.UtcNow,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            
            await _context.Entry(usuario).Reference(u=>u.Rol).LoadAsync();

            return new UsuarioReadDTO
            {
                Id = usuario.Id,
                Rol = new RolReadDTO
                {
                    Id = usuario.Rol.Id,
                    Nombre = usuario.Rol.Nombre
                },
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                Estado = usuario.Estado
            };
        }

        public async Task<bool> Update(int id, UsuarioUpdateDTO dto)
        {

            if (!EsMismoUsuario(id) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            
            var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario is null) throw new Exception($"Usuario no encontrado con el Usuario ID : {id}");

            // Hasheamos solo si enviamos la contraseña desde el frontend
            if (!string.IsNullOrEmpty(dto.PasswordHash))
            {
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            }
           
            usuario.Nombre = dto.Nombre;
            usuario.Email = dto.Email;
            usuario.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteByLogic(int id)
        {
            if (!EsMismoUsuario(id) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null) return false;

            usuario.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}