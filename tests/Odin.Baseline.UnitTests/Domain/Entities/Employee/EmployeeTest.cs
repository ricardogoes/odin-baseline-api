using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.Employee
{
    [Collection(nameof(EmployeeTestFixtureCollection))]
    public class EmployeeTest
    {
        private readonly EmployeeTestFixture _fixture;

        public EmployeeTest(EmployeeTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new employee")]
        [Trait("Domain", "Entities / Employee")]
        public void Instantiate()
        {
            var validEmployee = _fixture.GetValidEmployee();
            var employee = new DomainEntity.Employee(validEmployee.CustomerId, validEmployee.FirstName, validEmployee.LastName, validEmployee.Document, validEmployee.Email, validEmployee.DepartmentId);

            employee.Should().NotBeNull();
            employee.FirstName.Should().Be(validEmployee.FirstName);
            employee.LastName.Should().Be(validEmployee.LastName);
            employee.Email.Should().Be(validEmployee.Email);
            employee.Document.Should().Be(validEmployee.Document);
            employee.Id.Should().NotBeEmpty();
            employee.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = "ctor() should throw an error when first name is empty")]
        [Trait("Domain", "Entities / Employee")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenFirstNameIsEmpty(string? firstName)
        {
            var validEmployee = _fixture.GetValidEmployee();

            Action action = () => new DomainEntity.Employee(validEmployee.CustomerId, firstName!, validEmployee.LastName, validEmployee.Document, validEmployee.Email, validEmployee.DepartmentId);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("FirstName should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when last name is empty")]
        [Trait("Domain", "Entities / Employee")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenLastNameIsEmpty(string? lastName)
        {
            var validEmployee = _fixture.GetValidEmployee();

            Action action = () => new DomainEntity.Employee(validEmployee.CustomerId, validEmployee.FirstName, lastName!, validEmployee.Document, validEmployee.Email, validEmployee.DepartmentId);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("LastName should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when email is empty or invalid")]
        [Trait("Domain", "Entities / Employee")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("123123123")]
        [InlineData("email@email")]
        [InlineData("email.com")]
        public void InstantiateErrorWhenEmailIsEmptyOrInvalid(string? email)
        {
            var validEmployee = _fixture.GetValidEmployee();

            Action action = () => new DomainEntity.Employee(validEmployee.CustomerId, validEmployee.FirstName, validEmployee.LastName, validEmployee.Document, email!, validEmployee.DepartmentId);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Email should be a valid email");
        }

        [Fact(DisplayName = "ctor() should throw an error when Document is empty")]
        [Trait("Domain", "Entities / Employee")]
        public void InstantiateErrorWhenDocumentIsNull()
        {
            var validEmployee = _fixture.GetValidEmployee();

            Action action =
                () => new DomainEntity.Employee(validEmployee.CustomerId, validEmployee.FirstName, validEmployee.LastName, null, validEmployee.Email, validEmployee.DepartmentId);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Document should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Activate() should activate a employee")]
        [Trait("Domain", "Entities / Employee")]
        public void Activate()
        {
            var validEmployee = _fixture.GetValidEmployee();

            var employee = new DomainEntity.Employee(validEmployee.CustomerId, validEmployee.FirstName, validEmployee.LastName, validEmployee.Document, validEmployee.Email, validEmployee.DepartmentId);
            employee.Activate();

            employee.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a employee")]
        [Trait("Domain", "Entities / Employee")]
        public void Deactivate()
        {
            var validEmployee = _fixture.GetValidEmployee();

            var employee = new DomainEntity.Employee(validEmployee.CustomerId, validEmployee.FirstName, validEmployee.LastName, validEmployee.Document, validEmployee.Email, validEmployee.DepartmentId);
            employee.Deactivate();

            employee.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "ChangeAddress() should change address")]
        [Trait("Domain", "Entities / Customer")]
        public void ChangeAddress()
        {
            var employee = _fixture.GetValidEmployee();
            var address = _fixture.GetValidAddress();

            employee.ChangeAddress(address);

            employee.Address.Should().NotBeNull();
            employee.Address.StreetName.Should().Be(address.StreetName);
            employee.Address.StreetNumber.Should().Be(address.StreetNumber);
            employee.Address.Complement.Should().Be(address.Complement);
            employee.Address.Neighborhood.Should().Be(address.Neighborhood);
            employee.Address.ZipCode.Should().Be(address.ZipCode);
            employee.Address.City.Should().Be(address.City);
            employee.Address.State.Should().Be(address.State);
        }

        [Fact(DisplayName = "Create() should create a employee with valid CreatedAt and CreatedBy")]
        [Trait("Domain", "Entities / Employee")]
        public void Create()
        {
            var employee = _fixture.GetValidEmployee();
            var loggedUsername = _fixture.GetValidUsername();

            employee.Create(loggedUsername);

            employee.CreatedAt.Should().NotBeSameDateAs(default);
            employee.CreatedBy.Should().Be(loggedUsername);
        }

        [Fact(DisplayName = "Update() hould update a employee")]
        [Trait("Domain", "Entities / Employee")]
        public void Update()
        {
            var employee = _fixture.GetValidEmployee();
            var newEmployee = _fixture.GetValidEmployee();

            employee.Update(newEmployee.FirstName, newEmployee.LastName, newEmployee.Document, newEmployee.Email, newEmployee.CustomerId, newEmployee.DepartmentId);

            employee.FirstName.Should().Be(newEmployee.FirstName);
            employee.LastName.Should().Be(newEmployee.LastName);
            employee.Email.Should().Be(newEmployee.Email);
            employee.Document.Should().Be(newEmployee.Document);
            employee.CustomerId.Should().Be(newEmployee.CustomerId);
            employee.DepartmentId.Should().Be(newEmployee.DepartmentId);
        }

        [Theory(DisplayName = "Update() should throw an error when first name is empty")]
        [Trait("Domain", "Entities / Employee")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenFirstNameIsEmpty(string? name)
        {
            var employee = _fixture.GetValidEmployee();
            Action action =
                () => employee.Update(name!, employee.LastName, employee.Document, employee.Email);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("FirstName should not be empty or null");
        }

        [Theory(DisplayName = "Update() should throw an error when last name is empty")]
        [Trait("Domain", "Entities / Employee")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenLastNameIsEmpty(string? name)
        {
            var employee = _fixture.GetValidEmployee();
            Action action =
                () => employee.Update(employee.FirstName, name!, employee.Document, employee.Email);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("LastName should not be empty or null");
        }

        [Fact(DisplayName = "Update() should throw an error when Document is empty")]
        [Trait("Domain", "Entities / Employee")]
        public void UpdateErrorWhenLastDocumentIsEmpty()
        {
            var employee = _fixture.GetValidEmployee();

            Action action =() => employee.Update(employee.FirstName, employee.LastName, null, employee.Email);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Document should be a valid CPF or CNPJ");
        }

        [Theory(DisplayName = "Update() should throw an error when email is empty or invalid")]
        [Trait("Domain", "Entities / Employee")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("123123123")]
        [InlineData("email@email")]
        [InlineData("email.com")]
        public void UpdateErrorWhenLastEmailIsEmptyOrInvalid(string? email)
        {
            var employee = _fixture.GetValidEmployee();
            Action action =
                () => employee.Update(employee.FirstName, employee.LastName, employee.Document, email!);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Email should be a valid email");
        }

        [Fact(DisplayName = "AddHistoricPosition() should add historic positions to employee")]
        [Trait("Domain", "Entities / Employee")]
        public void AddHistoricPosition()
        {
            var employee = _fixture.GetValidEmployee();
            var historicPosition = _fixture.GetValidHistoricPosition();

            employee.AddHistoricPosition(historicPosition);

            employee.HistoricPositions.Should().HaveCount(1);
            employee.HistoricPositions.Should().Contain(historicPosition);
        }

        [Fact(DisplayName = "AddHistoricPosition() should add more than one historico position to employee")]
        [Trait("Domain", "Entities / Employee")]
        public void AddTwoDepartments()
        {
            var employee = _fixture.GetValidEmployee();
            var historicPosition1 = _fixture.GetValidHistoricPosition();
            var historicPosition2 = _fixture.GetValidHistoricPosition();

            employee.AddHistoricPosition(historicPosition1);
            employee.AddHistoricPosition(historicPosition2);

            employee.HistoricPositions.Should().HaveCount(2);
            employee.HistoricPositions.Should().Contain(historicPosition1);
            employee.HistoricPositions.Should().Contain(historicPosition2);
        }

        [Fact(DisplayName = "RemoveHistoricPosition() should remove a historic position")]
        [Trait("Domain", "Entities / Employee")]
        public void RemoveHistoricPosition()
        {
            var historicToRemove = _fixture.GetValidHistoricPosition();
            var employee = _fixture.GetValidEmployee(
                historicPositions: new List<DomainEntity.EmployeePositionHistory>()
                {
                    _fixture.GetValidHistoricPosition(),
                    _fixture.GetValidHistoricPosition(),
                    historicToRemove,
                    _fixture.GetValidHistoricPosition(),
                    _fixture.GetValidHistoricPosition()
                }
            );

            employee.RemoveHistoricPosition(historicToRemove);

            employee.HistoricPositions.Should().HaveCount(4);
            employee.HistoricPositions.Should().NotContain(historicToRemove);
        }

        [Fact(DisplayName = "RemoveAllHistoricPositions() should remove all historic positions")]
        [Trait("Domain", "Entities / Employee")]
        public void RemoveAllHistoricPositions()
        {
            var employee = _fixture.GetValidEmployee(
                 historicPositions: new List<DomainEntity.EmployeePositionHistory>()
                {
                    _fixture.GetValidHistoricPosition(),
                    _fixture.GetValidHistoricPosition(),
                    _fixture.GetValidHistoricPosition(),
                    _fixture.GetValidHistoricPosition()
                }
            );

            employee.RemoveAllHistoricPositions();

            employee.HistoricPositions.Should().HaveCount(0);
        }

        [Fact(DisplayName = "SetAuditLog() should set auditLog")]
        [Trait("Domain", "Entities / Customer")]
        public void AuditLog()
        {
            var employee = _fixture.GetValidEmployee();

            var createdAt = DateTime.Now;
            var createdBy = "unit.testing";
            var lastUpdatedAt = DateTime.Now;
            var lastUpdatedBy = "unit.testing";

            employee.SetAuditLog(createdAt, createdBy, lastUpdatedAt, lastUpdatedBy);

            employee.CreatedAt.Should().Be(createdAt);
            employee.CreatedBy.Should().Be(createdBy);
            employee.LastUpdatedAt.Should().Be(lastUpdatedAt);
            employee.LastUpdatedBy.Should().Be(lastUpdatedBy);
        }
    }
}
