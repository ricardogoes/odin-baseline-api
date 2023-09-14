using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Domain.Entities
{
    public class Employee : EntityWithDocument
    {
        public Guid CustomerId { get; private set; }

        public CustomerData? CustomerData { get; private set; }

        public Guid? DepartmentId { get; private set; }

        public DepartmentData? DepartmentData { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public Address? Address { get; private set; }

        public bool IsActive { get; set; }

        public IReadOnlyList<EmployeePositionHistory> HistoricPositions => _historicPositions.AsReadOnly();

        private readonly List<EmployeePositionHistory> _historicPositions;

        //TODO: Implementar loggedUsername
        private const string LOGGED_USERNAME = "ricardo.goes";

        public Employee(Guid id, Guid customerId, string firstName, string lastName, string document, string email, Guid? departmentId = null, bool isActive = true,
            List<EmployeePositionHistory>? positionsHistory = null)
           : base(document, id)
        {
            CustomerId = customerId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = isActive;

            _historicPositions = positionsHistory ?? new List<EmployeePositionHistory>();

            Validate();
        }

        public Employee(Guid customerId, string firstName, string lastName, string document, string email, Guid? departmentId = null, bool isActive = true)
            : base(document)
        {
            CustomerId = customerId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = isActive;

            _historicPositions = new List<EmployeePositionHistory>();

            Validate();
        }

        public void Create(string loggedUsername = LOGGED_USERNAME)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = loggedUsername; //TODO: Implementar loggedUser
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

            Validate();
        }

        public void Update(string newFirstName, string newLastName, string newDocument, string newEmail, Guid? newCustomerId = null, Guid? newDepartmentId = null, string loggedUsername = LOGGED_USERNAME)
        {
            CustomerId = newCustomerId ?? CustomerId;
            DepartmentId = newDepartmentId ?? DepartmentId;
            FirstName = newFirstName;
            LastName = newLastName;
            Document = newDocument;
            Email = newEmail;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

            Validate();
        }

        public void SetAuditLog(DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;

            Validate();
        }

        public void ChangeAddress(Address newAddress)
        {
            Address = newAddress;
            Validate();
        }

        public void Activate(string loggedUsername = LOGGED_USERNAME)
        {
            IsActive = true;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

            Validate();
        }

        public void Deactivate(string loggedUsername = LOGGED_USERNAME)
        {
            IsActive = false;
            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = loggedUsername; //TODO: Implementar loggedUser

            Validate();
        }

        public void AddHistoricPosition(EmployeePositionHistory positionHistory, string loggedUsername = LOGGED_USERNAME)
        {
            var actualPosition = HistoricPositions.FirstOrDefault(x => x.IsActual);
            actualPosition?.UpdateFinishDate(DateTime.UtcNow, loggedUsername);

            LoadHistoricPosition(positionHistory);

            Validate();
        }

        public void LoadCustomerData(CustomerData customerData)
        {
            CustomerData = customerData;
            Validate();
        }

        public void LoadDepartmentData(DepartmentData departmentData)
        {
            DepartmentData = departmentData;
            Validate();
        }

        public void LoadHistoricPosition(EmployeePositionHistory positionHistory)
        {
            _historicPositions.Add(positionHistory);
            Validate();
        }

        public void RemoveHistoricPosition(EmployeePositionHistory positionHistory)
        {
            _historicPositions.Remove(positionHistory);
            Validate();
        }

        public void RemoveAllHistoricPositions()
        {
            _historicPositions.Clear();
            Validate();
        }

        private void Validate()
        {
            DomainValidation.NotNullOrEmpty(CustomerId, nameof(CustomerId));
            DomainValidation.NotNullOrEmpty(FirstName, nameof(FirstName));
            DomainValidation.NotNullOrEmpty(LastName, nameof(LastName));
            DomainValidation.Email(Email, nameof(Email));
            CpfCnpjValidation.CpfCnpj(Document, nameof(Document));
        }
    }
}
