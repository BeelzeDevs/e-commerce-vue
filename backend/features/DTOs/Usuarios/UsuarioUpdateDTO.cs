namespace Backend.Features.DTOs
{
    public class UsuarioUpdateDTO
    {        
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Estado { get; set; }

    }
}