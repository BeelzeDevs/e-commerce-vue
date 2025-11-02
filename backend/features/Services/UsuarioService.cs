using Backend.Data;
using Backend.features.DTOs;
using Backend.features.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.features.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly EcommerceDbContext _context;
        public UsuarioService(EcommerceDbContext context)
        {
            _context = context;
        }
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
                    PasswordHash = u.PasswordHash,
                    FechaRegistro = u.FechaRegistro,
                    Estado = u.Estado
                }
            ).ToListAsync();
        }

        public async Task<UsuarioReadDTO?> GetById(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null) return null;
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
                PasswordHash = usuario.PasswordHash,
                FechaRegistro = usuario.FechaRegistro,
                Estado = usuario.Estado
            };
        }

        public async Task<UsuarioReadDTO?> Create(UsuarioCreateDTO dto)
        {
            var usuario = new Models.Usuario
            {
                RolId = 2, // Cliente
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                FechaRegistro = DateTime.Now,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            if (usuario is null) return null;
            return await GetById(usuario.Id) ?? throw new Exception("Error al crear Usuario");
        }
        public async Task<bool> Update(int id, UsuarioCreateDTO dto)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario is null) return false;

            usuario.Nombre = dto.Nombre;
            usuario.RolId = 2; // Cliente
            usuario.Email = dto.Email;
            usuario.PasswordHash = dto.PasswordHash;

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