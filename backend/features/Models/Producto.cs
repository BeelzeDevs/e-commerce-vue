using Backend.Features.Models;
namespace Backend.Features.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;

        public Categoria Categoria { get; set; } = null!;
        public ICollection<DetalleOrden> DetallesOrdenes { get; set; } = new List<DetalleOrden>();
    }
}