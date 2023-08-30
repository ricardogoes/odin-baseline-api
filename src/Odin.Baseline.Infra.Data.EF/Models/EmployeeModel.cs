namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
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

        public Guid CustomerId { get; set; }
        public CustomerModel Customer { get; set; }

        public Guid? DepartmentId { get; set; }
        public DepartmentModel Department { get; set; }

    }
}
