using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.SeedWork;
using Odin.Baseline.Domain.Validations;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Domain.Entities
{
    public class Employee : EntityWithDocument
    {
        public Guid? DepartmentId { get; private set; }

        public DepartmentData? DepartmentData { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public Address? Address { get; private set; }

        public bool IsActive { get; set; }

        public IReadOnlyList<EmployeePositionHistory> HistoricPositions => _historicPositions.AsReadOnly();

        private readonly List<EmployeePositionHistory> _historicPositions;

        public Employee(Guid id, string firstName, string lastName, string document, string email, Guid? departmentId = null, bool isActive = true,
            List<EmployeePositionHistory>? positionsHistory = null)
           : base(document, id)
        {
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = isActive;

            _historicPositions = positionsHistory ?? new List<EmployeePositionHistory>();

            Validate();
        }

        public Employee(string firstName, string lastName, string document, string email, Guid? departmentId = null, bool isActive = true)
            : base(document)
        {
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = isActive;

            _historicPositions = new List<EmployeePositionHistory>();

            Validate();
        }

        public void Update(string newFirstName, string newLastName, string newDocument, string newEmail, Guid? newDepartmentId = null)
        {
            DepartmentId = newDepartmentId ?? DepartmentId;
            FirstName = newFirstName;
            LastName = newLastName;
            Document = newDocument;
            Email = newEmail;

            Validate();
        }

        public void ChangeAddress(Address newAddress)
        {
            Address = newAddress;
            Validate();
        }

        public void Activate()
        {
            IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        public void AddHistoricPosition(EmployeePositionHistory positionHistory)
        {
            var actualPosition = HistoricPositions.FirstOrDefault(x => x.IsActual);
            actualPosition?.UpdateFinishDate(DateTime.UtcNow);

            LoadHistoricPosition(positionHistory);

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
            DomainValidation.NotNullOrEmpty(FirstName, nameof(FirstName));
            DomainValidation.NotNullOrEmpty(LastName, nameof(LastName));
            DomainValidation.Email(Email, nameof(Email));
            CpfCnpjValidation.CpfCnpj(Document, nameof(Document));
        }
    }
}
