using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.Customer
{
    [Collection(nameof(CustomerTestFixture))]
    public class CustomerTest
    {
        private readonly CustomerTestFixture _fixture;

        public CustomerTest(CustomerTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Instantiate()
        {
            var validCustomer = _fixture.GetValidCustomer();
            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document);

            customer.Should().NotBeNull();
            customer.Name.Should().Be(validCustomer.Name);
            customer.Document.Should().Be(validCustomer.Document);
            customer.Id.Should().NotBeEmpty();
            customer.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = "ctor() should instantiate an customer with IsActive status")]
        [Trait("Domain", "Entities / Customer")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            var validCustomer = _fixture.GetValidCustomer();
            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document, isActive: isActive);

            customer.Should().NotBeNull();
            customer.Name.Should().Be(validCustomer.Name);
            customer.Document.Should().Be(validCustomer.Document);
            customer.Id.Should().NotBeEmpty();
            customer.IsActive.Should().Be(isActive);
        }

        [Theory(DisplayName = "ctor() should throw an error when name is empty")]
        [Trait("Domain", "Entities / Customer")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validCustomer = _fixture.GetValidCustomer();

            Action action = () => new DomainEntity.Customer(name!, validCustomer.Document);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "ctor() should throw an error when Document is empty")]
        [Trait("Domain", "Entities / Customer")]
        public void InstantiateErrorWhenDocumentIsNull()
        {
            var validCustomer = _fixture.GetValidCustomer();

            Action action =
                () => new DomainEntity.Customer(validCustomer.Name, null!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Document should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Activate() should activate a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Activate()
        {
            var validCustomer = _fixture.GetValidCustomer();

            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document, isActive: false);
            customer.Activate();

            customer.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Deactivate()
        {
            var validCustomer = _fixture.GetValidCustomer();

            var customer = new DomainEntity.Customer(validCustomer.Name, validCustomer.Document, isActive: true);
            customer.Deactivate();

            customer.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "ChangeAddress() should change address")]
        [Trait("Domain", "Entities / Customer")]
        public void ChangeAddress()
        {
            var customer = _fixture.GetValidCustomer();
            var address = _fixture.GetValidAddress();

            customer.ChangeAddress(address);

            customer.Address.Should().NotBeNull();
            customer.Address.StreetName.Should().Be(address.StreetName);
            customer.Address.StreetNumber.Should().Be(address.StreetNumber);
            customer.Address.Complement.Should().Be(address.Complement);
            customer.Address.Neighborhood.Should().Be(address.Neighborhood);
            customer.Address.ZipCode.Should().Be(address.ZipCode);
            customer.Address.City.Should().Be(address.City);
            customer.Address.State.Should().Be(address.State);
        }

        [Fact(DisplayName = "Create() should create a customer with valid CreatedAt and CreatedBy")]
        [Trait("Domain", "Entities / Customer")]
        public void Create()
        {
            var customer = _fixture.GetValidCustomer();
            var loggedUsername = _fixture.GetValidUsername();

            customer.Create(loggedUsername);

            customer.CreatedAt.Should().NotBeSameDateAs(default);
            customer.CreatedBy.Should().Be(loggedUsername);
        }

        [Fact(DisplayName = "Update() hould update a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void Update()
        {
            var customer = _fixture.GetValidCustomer();
            var customerWithNewValues = _fixture.GetValidCustomer();

            customer.Update(customerWithNewValues.Name, customerWithNewValues.Document);

            customer.Name.Should().Be(customerWithNewValues.Name);
            customer.Document.Should().Be(customerWithNewValues.Document);
        }

        [Fact(DisplayName = "Update() should update only a name of a customer")]
        [Trait("Domain", "Entities / Customer")]
        public void UpdateOnlyName()
        {
            var customer = _fixture.GetValidCustomer();
            var newName = _fixture.GetValidCustomerName();
            var currentDocument = customer.Document;

            customer.Update(newName);

            customer.Name.Should().Be(newName);
            customer.Document.Should().Be(currentDocument);
        }

        [Theory(DisplayName = "Update() should throw an error when name is empty")]
        [Trait("Domain", "Entities / Customer")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var customer = _fixture.GetValidCustomer();
            Action action =
                () => customer.Update(name!);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "SetAuditLog() should set auditLog")]
        [Trait("Domain", "Entities / Customer")]
        public void AuditLog()
        {
            var customer = _fixture.GetValidCustomer();

            var createdAt = DateTime.Now;
            var createdBy = "unit.testing";
            var lastUpdatedAt = DateTime.Now;
            var lastUpdatedBy = "unit.testing";

            customer.SetAuditLog(createdAt, createdBy, lastUpdatedAt, lastUpdatedBy);

            customer.CreatedAt.Should().Be(createdAt);
            customer.CreatedBy.Should().Be(createdBy);
            customer.LastUpdatedAt.Should().Be(lastUpdatedAt);
            customer.LastUpdatedBy.Should().Be(lastUpdatedBy);
        }
    }
}
