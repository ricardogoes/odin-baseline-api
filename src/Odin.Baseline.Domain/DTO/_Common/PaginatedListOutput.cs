namespace Odin.Baseline.Domain.DTO.Common
{
    public class PaginatedListOutput<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }

    }
}
