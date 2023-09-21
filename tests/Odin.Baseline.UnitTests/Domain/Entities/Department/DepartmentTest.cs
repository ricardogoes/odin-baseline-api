using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.Department
{
    [Collection(nameof(DepartmentTestFixture))]
    public class DepartmentTest
    {
        private readonly DepartmentTestFixture _fixture;

        public DepartmentTest(DepartmentTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new department")]
        [Trait("Domain", "Entities / Department")]
        public void Instantiate()
        {
            var validDepartment = _fixture.GetValidDepartment();
            var department = new DomainEntity.Department(validDepartment.CustomerId, validDepartment.Name);

            department.Should().NotBeNull();
            department.Name.Should().Be(validDepartment.Name);
            department.Id.Should().NotBeEmpty();
            department.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "ctor() should throw an error when customer id is empty")]
        [Trait("Domain", "Entities / Department")]
        public void InstantiateErrorWhenCustomerIdIsEmpty()
        {
            var validDepartment = _fixture.GetValidDepartment();

            Action action = () => new DomainEntity.Department(Guid.Empty, validDepartment.Name);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("CustomerId should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when name is empty")]
        [Trait("Domain", "Entities / Department")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validDepartment = _fixture.GetValidDepartment();

            Action action = () => new DomainEntity.Department(validDepartment.CustomerId, name!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "Activate() should activate a department")]
        [Trait("Domain", "Entities / Department")]
        public void Activate()
        {
            var validDepartment = _fixture.GetValidDepartment();

            var department = new DomainEntity.Department(validDepartment.CustomerId, validDepartment.Name);
            department.Activate("unit.testing");

            department.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a department")]
        [Trait("Domain", "Entities / Department")]
        public void Deactivate()
        {
            var validDepartment = _fixture.GetValidDepartment();

            var department = new DomainEntity.Department(validDepartment.CustomerId, validDepartment.Name);
            department.Deactivate("unit.testing");

            department.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "Create() should create a department with valid CreatedAt and CreatedBy")]
        [Trait("Domain", "Entities / Department")]
        public void Create()
        {
            var department = _fixture.GetValidDepartment();
            var loggedUsername = _fixture.GetValidUsername();

            department.Create(loggedUsername);

            department.CreatedAt.Should().NotBeSameDateAs(default);
            department.CreatedBy.Should().Be(loggedUsername);
        }

        [Fact(DisplayName = "Update() hould update a department")]
        [Trait("Domain", "Entities / Department")]
        public void Update()
        {
            var department = _fixture.GetValidDepartment();
            var newDepartment = _fixture.GetValidDepartment();

            department.Update(newDepartment.Name, newDepartment.CustomerId, "unit.testing");

            department.Name.Should().Be(newDepartment.Name);
            department.CustomerId.Should().Be(newDepartment.CustomerId);
        }

        [Fact(DisplayName = "Update() should update only a name of a department")]
        [Trait("Domain", "Entities / Department")]
        public void UpdateOnlyName()
        {
            var department = _fixture.GetValidDepartment();
            var newName = _fixture.GetValidDepartmentName();

            department.Update(newName, department.CustomerId, "unit.testing");

            department.Name.Should().Be(newName);
            department.CustomerId.Should().Be(department.CustomerId);
        }

        [Fact(DisplayName = "Update() should throw an error when customerId is empty")]
        [Trait("Domain", "Entities / Department")]
        public void UpdateErrorWhenCustomerIdIsEmpty()
        {
            var department = _fixture.GetValidDepartment();
            Action action =
                () => department.Update(department.Name, Guid.Empty, "unit.testing");

            action.Should().Throw<EntityValidationException>()
                .WithMessage("CustomerId should not be empty or null");
        }

        [Theory(DisplayName = "Update() should throw an error when name is empty")]
        [Trait("Domain", "Entities / Department")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var department = _fixture.GetValidDepartment();
            Action action =
                () => department.Update(name!, department.CustomerId, "unit.testing");

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "LoadCustomerData() should load data of the customer related with department")]
        [Trait("Domain", "Entities / Department")]
        public void CustomerLoad()
        {
            var customer = _fixture.GetValidCustomer();
            var department = _fixture.GetValidDepartment(customer.Id);

            department.LoadCustomerData(new CustomerData(customer.Id, customer.Name));

            department.CustomerData.Should().NotBeNull();
            department.CustomerData!.Id.Should().Be(customer.Id);
            department.CustomerData.Name.Should().Be(customer.Name);
        }

        [Fact(DisplayName = "SetAuditLog() should set auditLog")]
        [Trait("Domain", "Entities / Department")]
        public void AuditLog()
        {
            var department = _fixture.GetValidDepartment();

            var createdAt = DateTime.Now;
            var createdBy = "unit.testing";
            var lastUpdatedAt = DateTime.Now;
            var lastUpdatedBy = "unit.testing";

            department.SetAuditLog(createdAt, createdBy, lastUpdatedAt, lastUpdatedBy);

            department.CreatedAt.Should().Be(createdAt);
            department.CreatedBy.Should().Be(createdBy);
            department.LastUpdatedAt.Should().Be(lastUpdatedAt);
            department.LastUpdatedBy.Should().Be(lastUpdatedBy);
        }
    }
}
