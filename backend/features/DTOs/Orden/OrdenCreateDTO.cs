namespace Backend.Features.DTOs
{
    public class OrdenCreateDTO
    {
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; } = 0;
    }
}