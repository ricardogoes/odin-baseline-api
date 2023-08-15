namespace Odin.Baseline.Domain.QueryModels
{
    public class DepartmentsQueryModel : ApiQueryModel
    {        
        public string Name { get; set; }
        public bool? IsActive { get; set; } = null;

        public DepartmentsQueryModel()
            : base()
        {                
        }

        public DepartmentsQueryModel(int pageNumber, int pageSize, string name, bool? isActive, string sort)
            : base(pageNumber, pageSize, sort)
        {
            Name = name;
            IsActive = isActive;
        }
    }
}
