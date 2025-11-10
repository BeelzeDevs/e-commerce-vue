using Backend.Features.Models;
namespace Backend.Features.Models
{
    public class Orden
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();

    }
}