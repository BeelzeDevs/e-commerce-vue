namespace Backend.Features.DTOs
{
    public class PaginationQuery
    {
        private const int MaxPageSize = 10;
        public int PageSize { get; set; } = 5;
        public int Page {get;set;} = 1;

        public void SetPageSize(int size)
        {
            if(size > 0 && size < MaxPageSize) PageSize = size;
            else PageSize = MaxPageSize;
            return;
        }

    }
}