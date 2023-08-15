namespace Odin.Baseline.Domain.Models
{
    public class PagedList<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalRecords { get; set; }
    }
}
