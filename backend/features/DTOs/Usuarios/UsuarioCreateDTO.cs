namespace Backend.features.DTOs
{
    public class UsuarioCreateDTO
    {        
        public int Id { get; set; }
        public int RolId { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}