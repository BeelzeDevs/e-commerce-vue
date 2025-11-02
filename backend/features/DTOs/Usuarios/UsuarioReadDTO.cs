using Backend.features.DTOs;

namespace Backend.features.DTOs
{
    public class UsuarioReadDTO
    {

        public int Id { get; set; }
        public RolReadDTO Rol { get; set; } = null!;
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Estado { get; set; }


    }
}