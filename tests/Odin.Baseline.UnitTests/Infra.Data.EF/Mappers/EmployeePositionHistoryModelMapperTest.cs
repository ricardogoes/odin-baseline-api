using FluentAssertions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Mappers
{
    [Collection(nameof(ModelMapperTestFixtureCollection))]
    public class EmployeePositionHistoryModelMapperTest
    {
        private readonly ModelMapperTestFixture _fixture;

        public EmployeePositionHistoryModelMapperTest(ModelMapperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ToEmployeePositionHistoryModel() should map an EmployeePositionHistory to EmployeePositionHistoryModel")]
        [Trait("Infra.Data.EF", "Mappers / EmployeePositionHistoryModelMapper")]
        public void MapEmployeePositionHistoryToEmployeePositionHistoryModel()
        {
            var employeeId = Guid.NewGuid();
            var positionHistory = _fixture.GetValidEmployeePositionHistory();
            var model = positionHistory.ToEmployeePositionHistoryModel(employeeId, Guid.NewGuid());

            model.Should().NotBeNull();
            model.EmployeeId.Should().Be(employeeId);
            model.PositionId.Should().Be(positionHistory.PositionId);
            model.Salary.Should().Be(positionHistory.Salary);
            model.StartDate.Should().Be(positionHistory.StartDate);
            model.FinishDate.Should().Be(positionHistory.FinishDate);
            model.IsActual.Should().Be(positionHistory.IsActual);
        }

        [Fact(DisplayName = "ToEmployeePositionHistoryModel() should map a list of positionHistorys to EmployeePositionHistoryModel")]
        [Trait("Infra.Data.EF", "Mappers / EmployeePositionHistoryModelMapper")]
        public void MapListEmployeePositionHistorysToEmployeePositionHistoryModel()
        {
            var employeeId = Guid.NewGuid();
            var positionHistory1 = _fixture.GetValidEmployeePositionHistory();            
            var positionHistory2 = _fixture.GetValidEmployeePositionHistory();
            var positionHistorys = new List<EmployeePositionHistory> { positionHistory1, positionHistory2 };

            var model = positionHistorys.ToEmployeePositionHistoryModel(employeeId, Guid.NewGuid());

            model.Should().NotBeNull();
            foreach (var positionHistory in model)
            {
                var positionHistoryToCompare = positionHistorys.FirstOrDefault(x => x.PositionId == positionHistory.PositionId);
                positionHistory.EmployeeId.Should().Be(employeeId);
                positionHistory.PositionId.Should().Be(positionHistory.PositionId);
                positionHistory.Salary.Should().Be(positionHistory.Salary);
                positionHistory.StartDate.Should().Be(positionHistory.StartDate);
                positionHistory.FinishDate.Should().Be(positionHistory.FinishDate);
                positionHistory.IsActual.Should().Be(positionHistory.IsActual);
            }
        }

        [Fact(DisplayName = "ToEmployeePositionHistory() should map an EmployeePositionHistoryModel to EmployeePositionHistory")]
        [Trait("Infra.Data.EF", "Mappers / EmployeePositionHistoryModelMapper")]
        public void MapEmployeePositionHistoryModelToEmployeePositionHistoryWithAddress()
        {
            var model = _fixture.GetValidEmployeePositionHistoryModel();
            var positionHistory = model.ToEmployeePositionHistory();

            positionHistory.Should().NotBeNull();
            positionHistory.PositionId.Should().Be(positionHistory.PositionId);
            positionHistory.Salary.Should().Be(positionHistory.Salary);
            positionHistory.StartDate.Should().Be(positionHistory.StartDate);
            positionHistory.FinishDate.Should().Be(positionHistory.FinishDate);
            positionHistory.IsActual.Should().Be(positionHistory.IsActual);
        }

        [Fact(DisplayName = "ToEmployeePositionHistory() should map a list of positionHistorys models to EmployeePositionHistory")]
        [Trait("Infra.Data.EF", "Mappers / EmployeePositionHistoryModelMapper")]
        public void MapListEmployeePositionHistorysModelToDepartmen()
        {            
            var positionHistoryModel1 = _fixture.GetValidEmployeePositionHistoryModel();
            var positionHistoryModel2 = _fixture.GetValidEmployeePositionHistoryModel();
            var positionHistorysModel = new List<EmployeePositionHistoryModel> { positionHistoryModel1, positionHistoryModel2 };

            var positionHistorys = positionHistorysModel.ToEmployeePositionHistory();

            positionHistorys.Should().NotBeNull();
            foreach (var positionHistory in positionHistorys)
            {
                var positionHistoryToCompare = positionHistorysModel.FirstOrDefault(x => x.PositionId == positionHistory.PositionId);
                positionHistory.Should().NotBeNull();
                positionHistory.PositionId.Should().Be(positionHistory.PositionId);
                positionHistory.Salary.Should().Be(positionHistory.Salary);
                positionHistory.StartDate.Should().Be(positionHistory.StartDate);
                positionHistory.FinishDate.Should().Be(positionHistory.FinishDate);
                positionHistory.IsActual.Should().Be(positionHistory.IsActual);
            }
        }
    }
}
