namespace Odin.Baseline.Domain.QueryModels
{
    public class CustomersQueryModel : ApiQueryModel
    {        
        public string Name { get; set; }
        public string Document { get; set; }
        public bool? IsActive { get; set; }

        public CustomersQueryModel(int pageNumber, int pageSize, string name, string document, bool? isActive, string sort)
            : base(pageNumber, pageSize, sort)
        {
            Name = name;
            Document = document;
            IsActive = isActive;
        }
    }
}
