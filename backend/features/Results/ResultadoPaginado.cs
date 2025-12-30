namespace Backend.Features.DTOs
{
    public class ResultadoPaginado<T>
    {
        public List<T> Items {get; set; } = new List<T>();
        public int Page {get;set; } = 1;
        public int PageSize {get;set;} = 5;
        public int TotalItems {get;set;} = 0;
        public int TotalPages {get;set;} = 0;
    }
}