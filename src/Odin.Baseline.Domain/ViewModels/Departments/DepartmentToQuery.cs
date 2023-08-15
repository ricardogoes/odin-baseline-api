namespace Odin.Baseline.Domain.ViewModels.Departments
{
    public class DepartmentToQuery
    {
        public int DepartmentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
