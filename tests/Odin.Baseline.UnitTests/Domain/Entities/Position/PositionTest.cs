using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.Position
{
    [Collection(nameof(PositionTestFixtureCollection))]
    public class PositionTest
    {
        private readonly PositionTestFixture _fixture;

        public PositionTest(PositionTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new position")]
        [Trait("Domain", "Entities / Position")]
        public void Instantiate()
        {
            var validPosition = _fixture.GetValidPosition();            
            var position = new DomainEntity.Position(customerId: validPosition.CustomerId, validPosition.Name, validPosition.BaseSalary);
            
            position.Should().NotBeNull();
            position.CustomerId.Should().Be(validPosition.CustomerId);
            position.Name.Should().Be(validPosition.Name);
            position.BaseSalary.Should().Be(validPosition.BaseSalary);
            position.Id.Should().NotBeEmpty();
            position.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "ctor() should throw an error when customerId is empty")]
        [Trait("Domain", "Entities / Position")]        
        public void InstantiateErrorWhenCustomerIdIsEmpty()
        {
            var validPosition = _fixture.GetValidPosition();
            var emptyGuid = Guid.Empty;

            Action action = () => _ = new DomainEntity.Position(emptyGuid, validPosition.Name, validPosition.BaseSalary);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("CustomerId should not be empty or null");
        }

        [Theory(DisplayName = "ctor() hould throw an error when name is empty")]
        [Trait("Domain", "Entities / Position")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validPosition = _fixture.GetValidPosition();

            Action action = () => new DomainEntity.Position(validPosition.CustomerId, name!, validPosition.BaseSalary);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "Activate() should activate a position")]
        [Trait("Domain", "Entities / Position")]
        public void Activate()
        {
            var validPosition = _fixture.GetValidPosition();

            var position = new DomainEntity.Position(validPosition.CustomerId, validPosition.Name, validPosition.BaseSalary);
            position.Activate();

            position.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a position")]
        [Trait("Domain", "Entities / Position")]
        public void Deactivate()
        {
            var validPosition = _fixture.GetValidPosition();

            var position = new DomainEntity.Position(validPosition.CustomerId, validPosition.Name, validPosition.BaseSalary);
            position.Deactivate();

            position.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "Create() should create a position with valid CreatedAt and CreatedBy")]
        [Trait("Domain", "Entities / Position")]
        public void Create()
        {
            var position = _fixture.GetValidPosition();
            var loggedUsername = _fixture.GetValidUsername();

            position.Create(loggedUsername);

            position.CreatedAt.Should().NotBeSameDateAs(default);
            position.CreatedBy.Should().Be(loggedUsername);
        }

        [Fact(DisplayName = "Update() should update a position")]
        [Trait("Domain", "Entities / Position")]
        public void Update()
        {
            var position = _fixture.GetValidPosition();
            var positionWithNewValues = _fixture.GetValidPosition();

            position.Update(positionWithNewValues.Name, positionWithNewValues.CustomerId, positionWithNewValues.BaseSalary);

            position.Name.Should().Be(positionWithNewValues.Name);
            position.BaseSalary.Should().Be(positionWithNewValues.BaseSalary);
        }

        [Fact(DisplayName = "Update() should update only a name of a position")]
        [Trait("Domain", "Entities / Position")]
        public void UpdateOnlyName()
        {
            var position = _fixture.GetValidPosition();
            var newName = _fixture.GetValidPositionName();
            var currentBaseSalary = position.BaseSalary;

            position.Update(newName);

            position.Name.Should().Be(newName);
            position.BaseSalary.Should().Be(currentBaseSalary);
        }

        [Theory(DisplayName = "Should throw an error when name is empty")]
        [Trait("Domain", "Entities / Position")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var position = _fixture.GetValidPosition();

            Action action = () => position.Update(name!);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "LoadCustomerData() should load data of the customer related with position")]
        [Trait("Domain", "Entities / Position")]
        public void CustomerLoad()
        {
            var customer = _fixture.GetValidCustomer();
            var employee = _fixture.GetValidPosition(customer.Id);

            employee.LoadCustomerData(new CustomerData(customer.Id, customer.Name));

            employee.CustomerData.Should().NotBeNull();
            employee.CustomerData.Id.Should().Be(customer.Id);
            employee.CustomerData.Name.Should().Be(customer.Name);
        }

        [Fact(DisplayName = "SetAuditLog() should set auditLog")]
        [Trait("Domain", "Entities / Customer")]
        public void AuditLog()
        {
            var position = _fixture.GetValidPosition();

            var createdAt = DateTime.Now;
            var createdBy = "unit.testing";
            var lastUpdatedAt = DateTime.Now;
            var lastUpdatedBy = "unit.testing";

            position.SetAuditLog(createdAt, createdBy, lastUpdatedAt, lastUpdatedBy);

            position.CreatedAt.Should().Be(createdAt);
            position.CreatedBy.Should().Be(createdBy);
            position.LastUpdatedAt.Should().Be(lastUpdatedAt);
            position.LastUpdatedBy.Should().Be(lastUpdatedBy);
        }
    }
}
