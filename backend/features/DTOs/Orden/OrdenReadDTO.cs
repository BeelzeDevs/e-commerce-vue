namespace Backend.Features.DTOs
{
    public class OrdenReadDTO
    {
        
        public int Id { get; set; }
        public UsuarioReadDTO Usuario { get; set; } = new UsuarioReadDTO();
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado {get;set;} = string.Empty;

    }
}