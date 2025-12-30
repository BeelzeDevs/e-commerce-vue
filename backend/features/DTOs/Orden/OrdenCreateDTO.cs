namespace Backend.Features.DTOs
{
    public class OrdenCreateDTO
    {
        public List<CarritoItem> CarritoItems { get; set;} = new List<CarritoItem>();
        public DateTime? Fecha { get; set; } = DateTime.UtcNow;
        
    }
}