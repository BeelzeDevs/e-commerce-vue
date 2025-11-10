using Backend.Features.Models;
namespace Backend.Features.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Estado { get; set; } = true;


        public Rol Rol { get; set; } = null!;
        public ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
    }
}