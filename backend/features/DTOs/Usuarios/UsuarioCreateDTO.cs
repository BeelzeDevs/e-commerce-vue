namespace Backend.Features.DTOs
{
    public class UsuarioCreateDTO
    {        
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

    }
}