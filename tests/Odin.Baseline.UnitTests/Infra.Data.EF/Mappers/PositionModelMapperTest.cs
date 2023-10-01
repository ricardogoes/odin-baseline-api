using FluentAssertions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Mappers
{
    [Collection(nameof(ModelMapperTestFixtureCollection))]
    public class PositionModelMapperTest
    {
        private readonly ModelMapperTestFixture _fixture;

        public PositionModelMapperTest(ModelMapperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ToPositionModel() should map an Position to PositionModel")]
        [Trait("Infra.Data.EF", "Mappers / PositionModelMapper")]
        public void MapPositionToPositionModel()
        {
            var position = _fixture.GetValidPosition();
            var model = position.ToPositionModel(Guid.NewGuid());

            model.Should().NotBeNull();
            model.Id.Should().Be(position.Id);
            model.Name.Should().Be(position.Name);
            model.BaseSalary.Should().Be(position.BaseSalary);
            model.IsActive.Should().Be(position.IsActive);
        }

        [Fact(DisplayName = "ToPositionModel() should map a list of positions to PositionModel")]
        [Trait("Infra.Data.EF", "Mappers / PositionModelMapper")]
        public void MapListPositionsToPositionModel()
        {
            var position1 = _fixture.GetValidPosition();            
            var position2 = _fixture.GetValidPosition();
            var positions = new List<Position> { position1, position2 };

            var model = positions.ToPositionModel(Guid.NewGuid());

            model.Should().NotBeNull();
            foreach (var position in model)
            {
                var positionToCompare = positions.FirstOrDefault(x => x.Id == position.Id);
                position.Id.Should().Be(positionToCompare!.Id);
                position.Name.Should().Be(positionToCompare.Name);
                position.BaseSalary.Should().Be(positionToCompare.BaseSalary);
                position.IsActive.Should().Be(positionToCompare.IsActive);
            }
        }

        [Fact(DisplayName = "ToPosition() should map an PositionModel to Position")]
        [Trait("Infra.Data.EF", "Mappers / PositionModelMapper")]
        public void MapPositionModelToPositionWithAddress()
        {
            var model = _fixture.GetValidPositionModel();
            var position = model.ToPosition();

            position.Should().NotBeNull();
            position.Id.Should().Be(model.Id);
            position.Name.Should().Be(model.Name);
            position.BaseSalary.Should().Be(model.BaseSalary);
            position.IsActive.Should().Be(model.IsActive);
        }

        [Fact(DisplayName = "ToPosition() should map a list of positions models to Position")]
        [Trait("Infra.Data.EF", "Mappers / PositionModelMapper")]
        public void MapListPositionsModelToDepartmen()
        {            
            var positionModel1 = _fixture.GetValidPositionModel();
            var positionModel2 = _fixture.GetValidPositionModel();
            var positionsModel = new List<PositionModel> { positionModel1, positionModel2 };

            var positions = positionsModel.ToPosition();

            positions.Should().NotBeNull();
            foreach (var position in positions)
            {
                var positionToCompare = positionsModel.FirstOrDefault(x => x.Id == position.Id);
                position.Should().NotBeNull();
                position.Id.Should().Be(positionToCompare!.Id);
                position.Name.Should().Be(positionToCompare.Name);
                position.BaseSalary.Should().Be(positionToCompare.BaseSalary);
                position.IsActive.Should().Be(positionToCompare.IsActive);
            }
        }
    }
}
