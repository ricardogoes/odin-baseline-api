using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Odin.Baseline.Domain.Models
{
    public class PagedApiResponse<T> where T: class
    {
        public string State { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }
                
        public int PageNumber { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalPages { get; set; }
        
        public int TotalRecords { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<T> Data { get; set; }

        public PagedApiResponse(string state, string message, PagedList<T> pagedList, int pageNumber, int pageSize)
        {
            State = state;
            Message = message;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = pagedList.TotalRecords < pageSize ? 1 : pagedList.TotalRecords/pageSize;
            TotalRecords = pagedList.TotalRecords;
            Data = pagedList.Items;
        }

        public PagedApiResponse(string state, PagedList<T> pagedList, int pageNumber, int pageSize)
        {
            State = state;            
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = pagedList.TotalRecords < pageSize ? 1 : pagedList.TotalRecords / pageSize;
            TotalRecords = pagedList.TotalRecords;
            Data = pagedList.Items;
        }
    }
}
