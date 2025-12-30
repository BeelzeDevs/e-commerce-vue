using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Features.Auth.DTOs;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Features.Results;

namespace Backend.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly EcommerceDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(EcommerceDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginRespDTO> LoginAsync(UsuarioLoginDTO dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario is null) throw new UnauthorizedAccessException("❌ Email incorrecto");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
            if (!valid) throw new UnauthorizedAccessException("❌ Datos incorrectos");

            var keyStr = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiresMinutes = _config.GetValue<int?>("Jwt:ExpiresInMinutes") ?? 240;

            if (string.IsNullOrWhiteSpace(keyStr))
                throw new InvalidOperationException("la key de JWT no esta configurada");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("UsuarioId", usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            };

            var expiration = DateTime.UtcNow.AddMinutes(expiresMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new LoginRespDTO
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Rol = usuario.Rol.Nombre,
                Expiration = expiration   
            };

        }
    }
}
