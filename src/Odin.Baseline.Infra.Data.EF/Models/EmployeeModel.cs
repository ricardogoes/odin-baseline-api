namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; private set; }        
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }
        public string? StreetName { get; private set; }
        public int? StreetNumber { get; private set; }
        public string? Complement { get; private set; }
        public string? Neighborhood { get; private set; }
        public string? ZipCode { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public Guid CustomerId { get; private set; }
        public CustomerModel? Customer { get; private set; }

        public Guid? DepartmentId { get; private set; }
        public DepartmentModel? Department { get; private set; }

        public ICollection<EmployeePositionHistoryModel> HistoricPositions { get; private set; } = new List<EmployeePositionHistoryModel>();

        public EmployeeModel(Guid id, string firstName, string lastName, string document, string email, string? streetName, int? streetNumber, string? complement, 
            string? neighborhood, string? zipCode, string? city, string? state, bool isActive, DateTime createdAt, string createdBy, 
            DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId, Guid? departmentId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
            StreetName = streetName;
            StreetNumber = streetNumber;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            CustomerId = customerId;
            DepartmentId = departmentId;
        }

        public EmployeeModel(Guid id, string firstName, string lastName, string document, string email, string? streetName, int? streetNumber, string? complement,
            string? neighborhood, string? zipCode, string? city, string? state, bool isActive, DateTime createdAt, string createdBy,
            DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId, Guid? departmentId, CustomerModel customerModel)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
            StreetName = streetName;
            StreetNumber = streetNumber;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            CustomerId = customerId;
            DepartmentId = departmentId;
            Customer = customerModel;
        }

        public EmployeeModel(Guid id, string firstName, string lastName, string document, string email, bool isActive, 
            DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId, Guid? departmentId = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            CustomerId = customerId;
            DepartmentId = departmentId;
        }

        public EmployeeModel(Guid id, string firstName, string lastName, string document, string email, bool isActive,
            DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid customerId, Guid? departmentId, CustomerModel customerModel)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            CustomerId = customerId;
            DepartmentId = departmentId;
            Customer = customerModel;
        }
    }
}
