namespace Odin.Baseline.Domain.QueryModels
{
    public class ApiQueryModel
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }

        public ApiQueryModel()
        {
            PageNumber = 1;
            PageSize = 10;
            Sort = "lastUpdatedBy desc";
        }

        public ApiQueryModel(int pageNumber, int pageSize, string sort)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
            Sort = sort;
        }
    }
}
