namespace Backend.Features.DTOs
{
    public class OrdenUpdateDTO
    {
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } 
        public decimal Total { get; set; }
    }
}