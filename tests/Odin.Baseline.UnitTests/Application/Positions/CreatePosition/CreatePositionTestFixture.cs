using Odin.Baseline.Application.Positions.CreatePosition;
using Odin.Baseline.UnitTests.Application.Positions.Common;

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
                customerId: Guid.NewGuid(),
                name: GetValidName(),
                baseSalary: GetValidBaseSalary(),                
                loggedUsername: GetValidUsername()
            );

        public CreatePositionInput GetCreatePositionInputWithEmptyCustomerId()
            => new
            (
                customerId: Guid.Empty,
                name: GetValidName(),
                baseSalary: GetValidBaseSalary(),
                loggedUsername: GetValidUsername()
            );

        public CreatePositionInput GetCreatePositionInputWithEmptyName()
            => new
            (
                customerId: Guid.NewGuid(),
                name: "",
                baseSalary: GetValidBaseSalary(),
                loggedUsername: GetValidUsername()
            );

        public CreatePositionInput GetCreatePositionInputWithEmptyLoggedUsername()
           => new
           (
               customerId: Guid.NewGuid(),
               name: GetValidName(),
               baseSalary: GetValidBaseSalary(),
               loggedUsername: ""
           );
    }
}
