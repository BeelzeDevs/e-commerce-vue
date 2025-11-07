using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.features.Auth.DTOs;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Auth.Services
{
    public class AuthService: IAuthService
    {
        private readonly EcommerceDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(EcommerceDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDTO?> LoginAsync(UsuarioLoginDTO dto)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => dto.Email == u.Email && u.PasswordHash == dto.Password);
            if (usuario is null) return null;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
                new Claim("UsuarioId",usuario.Id.ToString())
            };

            var expiration = DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new AuthResponseDTO
            {
                Email = usuario.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}