using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
using DomainEntity = Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.UnitTests.Domain.Entities.EmployeePositionHistory
{
    [Collection(nameof(EmployeePositionHistoryTestFixtureCollection))]
    public class EmployeePositionHistoryTest
    {
        private readonly EmployeePositionHistoryTestFixture _fixture;

        public EmployeePositionHistoryTest(EmployeePositionHistoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new position history")]
        [Trait("Domain", "Entities / EmployeePositionHistory")]
        public void Instantiate()
        {
            var validEmployeePositionHistory = _fixture.GetValidEmployeePositionHistory();
            var positionHistory = new DomainEntity.EmployeePositionHistory(validEmployeePositionHistory.PositionId, validEmployeePositionHistory.Salary, validEmployeePositionHistory.StartDate, 
                validEmployeePositionHistory.FinishDate, validEmployeePositionHistory.IsActual);

            positionHistory.Should().NotBeNull();
            positionHistory.PositionId.Should().Be(validEmployeePositionHistory.PositionId);
            positionHistory.Salary.Should().Be(validEmployeePositionHistory.Salary);
            positionHistory.IsActual.Should().Be(validEmployeePositionHistory.IsActual);
            positionHistory.Id.Should().NotBeEmpty();            
        }

        [Fact(DisplayName = "ctor() should throw an error when PositionId is empty")]
        [Trait("Domain", "Entities / EmployeePositionHistory")]
        public void InstantiateErrorWhenFirstPositionIdIsEmpty()
        {
            var validEmployeePositionHistory = _fixture.GetValidEmployeePositionHistory();

            Action action = () => new DomainEntity.EmployeePositionHistory(Guid.Empty, validEmployeePositionHistory.Salary, validEmployeePositionHistory.StartDate,
                validEmployeePositionHistory.FinishDate, validEmployeePositionHistory.IsActual);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("PositionId should not be empty or null");
        }

        [Fact(DisplayName = "ctor() should throw an error when salary is empty")]
        [Trait("Domain", "Entities / EmployeePositionHistory")]
        public void InstantiateErrorWhenSalaryIsEmpty()
        {
            var validEmployeePositionHistory = _fixture.GetValidEmployeePositionHistory();

            Action action = () => new DomainEntity.EmployeePositionHistory(validEmployeePositionHistory.PositionId, -1, validEmployeePositionHistory.StartDate,
                validEmployeePositionHistory.FinishDate, validEmployeePositionHistory.IsActual);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Salary should not be empty or null");
        }

        [Fact(DisplayName = "ctor() should throw an error when start date is empty")]
        [Trait("Domain", "Entities / EmployeePositionHistory")]
        public void InstantiateErrorWhenStartDateIsEmpty()
        {
            var validEmployeePositionHistory = _fixture.GetValidEmployeePositionHistory();

            Action action = () => new DomainEntity.EmployeePositionHistory(validEmployeePositionHistory.PositionId, validEmployeePositionHistory.Salary, default,
                validEmployeePositionHistory.FinishDate, validEmployeePositionHistory.IsActual);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("StartDate should not be empty or null");
        }

        [Fact(DisplayName = "UpdateFinishDate() should update finish date")]
        [Trait("Domain", "Entities / EmployeePositionHistory")]
        public void ShouldUpdateFinishDate()
        {
            var validEmployeePositionHistory = _fixture.GetValidEmployeePositionHistory();

            var finishDate = DateTime.Now;

            validEmployeePositionHistory.UpdateFinishDate(finishDate);

            validEmployeePositionHistory.Should().NotBeNull();
            validEmployeePositionHistory.FinishDate.Should().Be(finishDate);
            validEmployeePositionHistory.IsActual.Should().BeFalse();
            
        }
    }
}
