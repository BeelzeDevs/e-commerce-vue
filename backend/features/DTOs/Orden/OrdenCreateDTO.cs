namespace Backend.features.DTOs
{
    public class OrdenCreateDTO
    {
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
    }
}