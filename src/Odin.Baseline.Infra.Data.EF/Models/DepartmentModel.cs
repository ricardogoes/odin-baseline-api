namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class DepartmentModel
    {
        public Guid Id { get; set; }        
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerModel Customer { get; set; }

        public ICollection<EmployeeModel> Employees { get; }
    }
}
