using Odin.Baseline.Application.Positions.CreatePosition;

namespace Odin.Baseline.UnitTests.Application.Positions.CreatePosition
{
    [CollectionDefinition(nameof(CreatePositionTestFixtureCollection))]
    public class CreatePositionTestFixtureCollection : ICollectionFixture<CreatePositionTestFixture>
    { }

    public class CreatePositionTestFixture : PositionBaseFixture
    {
        public CreatePositionTestFixture()
            : base() { }

        public CreatePositionInput GetValidCreatePositionInput()
            => new
            (
                name: GetValidName(),
                baseSalary: GetValidBaseSalary()
            );

        public CreatePositionInput GetCreatePositionInputWithEmptyName()
            => new
            (
                name: "",
                baseSalary: GetValidBaseSalary()
            );
    }
}
