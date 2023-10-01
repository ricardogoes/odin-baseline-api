using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
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
            var department = new DomainEntity.Department(validDepartment.Name);

            department.Should().NotBeNull();
            department.Name.Should().Be(validDepartment.Name);
            department.Id.Should().NotBeEmpty();
            department.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = "ctor() should throw an error when name is invalid")]
        [Trait("Domain", "Entities / Department")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validDepartment = _fixture.GetValidDepartment();

            Action action = () => new DomainEntity.Department(name!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "Activate() should activate a department")]
        [Trait("Domain", "Entities / Department")]
        public void Activate()
        {
            var validDepartment = _fixture.GetValidDepartment();

            var department = new DomainEntity.Department(validDepartment.Name);
            department.Activate();

            department.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a department")]
        [Trait("Domain", "Entities / Department")]
        public void Deactivate()
        {
            var validDepartment = _fixture.GetValidDepartment();

            var department = new DomainEntity.Department(validDepartment.Name);
            department.Deactivate();

            department.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "Update() should update a department")]
        [Trait("Domain", "Entities / Department")]
        public void Update()
        {
            var department = _fixture.GetValidDepartment();
            var newDepartment = _fixture.GetValidDepartment();

            department.Update(newDepartment.Name);

            department.Name.Should().Be(newDepartment.Name);
        }

        [Theory(DisplayName = "Update() should throw an error when name is invalid")]
        [Trait("Domain", "Entities / Department")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var department = _fixture.GetValidDepartment();
            Action action =
                () => department.Update(name!);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
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
