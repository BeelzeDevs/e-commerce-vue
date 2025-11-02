namespace Backend.features.DTOs
{
    public class ProductoReadDTO
    {
        public int Id { get; set; }
        public CategoriaReadDTO Categoria { get; set; } = null!;
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;

    }
}