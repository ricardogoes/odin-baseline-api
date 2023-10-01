using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
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
            var position = new DomainEntity.Position(validPosition.Name, validPosition.BaseSalary);
            
            position.Should().NotBeNull();
            position.Name.Should().Be(validPosition.Name);
            position.BaseSalary.Should().Be(validPosition.BaseSalary);
            position.Id.Should().NotBeEmpty();
            position.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = "ctor() hould throw an error when name is empty")]
        [Trait("Domain", "Entities / Position")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validPosition = _fixture.GetValidPosition();

            Action action = () => new DomainEntity.Position(name!, validPosition.BaseSalary);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = "Activate() should activate a position")]
        [Trait("Domain", "Entities / Position")]
        public void Activate()
        {
            var validPosition = _fixture.GetValidPosition();

            var position = new DomainEntity.Position(validPosition.Name, validPosition.BaseSalary);
            position.Activate();

            position.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = "Deactivate() should deactivate a position")]
        [Trait("Domain", "Entities / Position")]
        public void Deactivate()
        {
            var validPosition = _fixture.GetValidPosition();

            var position = new DomainEntity.Position(validPosition.Name, validPosition.BaseSalary);
            position.Deactivate();

            position.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = "Update() should update a position")]
        [Trait("Domain", "Entities / Position")]
        public void Update()
        {
            var position = _fixture.GetValidPosition();
            var positionWithNewValues = _fixture.GetValidPosition();

            position.Update(positionWithNewValues.Name, positionWithNewValues.BaseSalary);

            position.Name.Should().Be(positionWithNewValues.Name);
            position.BaseSalary.Should().Be(positionWithNewValues.BaseSalary);
        }

        [Theory(DisplayName = "Update() should throw an error when name is empty")]
        [Trait("Domain", "Entities / Position")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var position = _fixture.GetValidPosition();

            Action action = () => position.Update(name!, position.BaseSalary);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
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
