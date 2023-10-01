using Odin.Baseline.Application.Positions.UpdatePosition;
using Odin.Baseline.EndToEndTests.Controllers.Positions;

namespace Odin.Baseline.EndToEndTests.Controllers.Positions.UpdatePosition
{
    [CollectionDefinition(nameof(UpdatePositionApiTestCollection))]
    public class UpdatePositionApiTestCollection : ICollectionFixture<UpdatePositionApiTestFixture>
    { }

    public class UpdatePositionApiTestFixture : PositionBaseFixture
    {
        public UpdatePositionApiTestFixture()
            : base()
        { }

        public UpdatePositionInput GetValidInput(Guid id)
            => new
            (
                id: id,
                name: GetValidPositionName(),
                baseSalary: 10_000
            );

        public UpdatePositionInput GetInputWithIdEmpty()
            => new
            (
                id: Guid.Empty,
                name: GetValidPositionName(),
                baseSalary: 10_000
            );


        public UpdatePositionInput GetInputWithNameEmpty(Guid id)
            => new
            (
                id: id,
                name: "",
                baseSalary: 10_000
            );
    }
}
