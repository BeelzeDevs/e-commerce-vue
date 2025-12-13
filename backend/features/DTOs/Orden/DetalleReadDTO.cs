using Backend.Features.DTOs;

namespace Backend.Features.DTOs
{
    public class DetalleReadDTO
    {
        public int OrdenId { get; set; }
        public ProductoReadDTO Producto { get; set; } = null!;
        public decimal Precio_Producto {get;set;}
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}