namespace Backend.features.DTOs
{
    public class OrdenReadDTO
    {
        
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }

    }
}