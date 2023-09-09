namespace Odin.Baseline.Domain.DTO.Common
{
    public abstract class PaginatedListInput
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }

        public PaginatedListInput()
        {
            PageNumber = 1;
            PageSize = 10;
            Sort = "lastUpdatedAt desc";
        }

        public PaginatedListInput(int pageNumber, int pageSize, string sort)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
            Sort = sort ?? "lastUpdatedAt desc";
        }
    }
}
