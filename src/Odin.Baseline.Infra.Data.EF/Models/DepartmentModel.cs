namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class DepartmentModel : BaseModel
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public ICollection<EmployeeModel> Employees { get; } = new List<EmployeeModel>();

        public DepartmentModel(Guid id, string name, bool isActive, DateTime createdAt, string createdBy,
            DateTime lastUpdatedAt, string lastUpdatedBy, Guid tenantId)
            : base(id, createdAt, createdBy, lastUpdatedAt, lastUpdatedBy, tenantId)
        {
            Name = name;
            IsActive = isActive;
        }         
    }
}
