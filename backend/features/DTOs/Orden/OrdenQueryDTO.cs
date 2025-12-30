namespace Backend.Features.DTOs
{
    public class OrdenQueryDTO : PaginationQuery
    {
        public DateTime? Fecha {get;set;} = null;
        public string? Estado {get;set;} = null;
        public string? SearchUsuario {get;set;} = null;
        public int? RolId {get;set;} = null;
    }
}