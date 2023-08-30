namespace Odin.Baseline.Application.Common
{
    public class PaginatedListHelper
    {
        public static int GetTotalPages(int totalItems, int pageSize)
            => totalItems < pageSize 
            ? 1 
            : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalItems) / Convert.ToDecimal(pageSize)));
    }
}
