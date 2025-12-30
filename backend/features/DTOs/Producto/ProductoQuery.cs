namespace Backend.Features.DTOs
{
    public class ProductoQuery : PaginationQuery
    {
        
        public int? CategoriaId { get; set; } = null!;
        public string? Search { get; set; } = string.Empty;
        public bool? Estado { get; set; } = null;
    }
}