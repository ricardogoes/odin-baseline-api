namespace Odin.Baseline.Domain.ViewModels.Employees
{
    public class EmployeeToQuery
    {
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? CompanyPositionId { get; set; }
        public string CompanyPositionName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal? Salary { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
