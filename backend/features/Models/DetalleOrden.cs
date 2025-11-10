using Backend.Features.Models;
namespace Backend.Features.Models
{
    public class DetalleOrden
    {
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }

        public Orden Orden { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
        
    }
}