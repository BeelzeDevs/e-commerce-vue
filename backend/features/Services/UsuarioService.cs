using System.Security.Claims;
using Backend.Data;
using Backend.Features.DTOs;
using Backend.Features.Models;
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
            return int.TryParse(claim, out var id) ? id : 0;
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
            ).ToListAsync();
        }

        public async Task<UsuarioReadDTO> GetById(int id)
        {
            if (!EsMismoUsuario(id) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");

            var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null) throw new Exception($"Usuario no encontrado con el Usuario ID : {id}");
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

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            var usuario = new Models.Usuario
            {
                RolId = (int)RolesEnum.Cliente,
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                FechaRegistro = DateTime.Now,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return await GetById(usuario.Id) ?? throw new Exception("Error al crear Usuario");
        }
        
        public async Task<bool> Update(int id, UsuarioUpdateDTO dto)
        {

            if (!EsMismoUsuario(id) && !EsAdmin()) throw new UnauthorizedAccessException("Acceso No Autorizado");
            
            var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario is null) throw new Exception($"Usuario no encontrado con el Usuario ID : {id}");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            usuario.Nombre = dto.Nombre;
            usuario.RolId = (int)RolesEnum.Cliente;
            usuario.Email = dto.Email;
            usuario.PasswordHash = hashedPassword;
            usuario.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteByLogic(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null) return false;

            usuario.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}