namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class EmployeeModel : BaseModel
    {
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
        
        public Guid? DepartmentId { get; private set; }
        public DepartmentModel? Department { get; private set; }
        
        public ICollection<EmployeePositionHistoryModel> HistoricPositions { get; private set; } = new List<EmployeePositionHistoryModel>();

        public EmployeeModel(Guid id, string firstName, string lastName, string document, string email, string? streetName, int? streetNumber, string? complement, 
            string? neighborhood, string? zipCode, string? city, string? state, bool isActive, DateTime createdAt, string createdBy, 
            DateTime lastUpdatedAt, string lastUpdatedBy, Guid tenantId, Guid? departmentId)
            : base(id, createdAt, createdBy, lastUpdatedAt, lastUpdatedBy, tenantId)
        {
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
            DepartmentId = departmentId;
        }

        public EmployeeModel(Guid id, string firstName, string lastName, string document, string email, bool isActive, 
            DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid tenantId, Guid? departmentId = null)
            : base(id, createdAt, createdBy, lastUpdatedAt, lastUpdatedBy, tenantId)
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
            TenantId = tenantId;
            DepartmentId = departmentId;
        }
    }
}
