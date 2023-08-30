namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string StreetName { get; set; }
        public int? StreetNumber { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public ICollection<DepartmentModel> Departments { get; }
        public ICollection<EmployeeModel> Employees { get; }
        public ICollection<PositionModel> Positions { get; }
    }
}
