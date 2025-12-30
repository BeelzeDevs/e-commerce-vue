namespace Backend.Features.DTOs
{
    public class UsuarioQueryDTO : PaginationQuery
    {
        public int? RolId {get;set;} = null;
        public string? Search {get;set;} = null;
        public DateTime? Fecha {get;set;} = null;
        public bool? Estado {get;set;} = null;


    }
}