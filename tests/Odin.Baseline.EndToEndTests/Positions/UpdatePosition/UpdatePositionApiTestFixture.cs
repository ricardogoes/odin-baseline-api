using Odin.Baseline.Api.Models.Positions;
using Odin.Baseline.Application.Positions.UpdatePosition;
using Odin.Baseline.EndToEndTests.Positions.Common;

namespace Odin.Baseline.EndToEndTests.Positions.UpdatePosition
{
    [CollectionDefinition(nameof(UpdatePositionApiTestCollection))]
    public class UpdatePositionApiTestCollection : ICollectionFixture<UpdatePositionApiTestFixture>
    { }

    public class UpdatePositionApiTestFixture : PositionBaseFixture
    {
        public UpdatePositionApiTestFixture()
            : base()
        { }

        public UpdatePositionInput GetValidInput(Guid id, Guid? customerId = null)
            => new
            (
                id: id,
                customerId: customerId ?? Guid.NewGuid(),
                name: GetValidName(),
                baseSalary: 10_000,
                loggedUsername: GetValidUsername()
            );

        public UpdatePositionInput GetInputWithIdEmpty()
            => new
            (
                id: Guid.Empty,
                customerId: Guid.NewGuid(),
                name: GetValidName(),
                baseSalary: 10_000,
                loggedUsername: GetValidUsername()
            );


        public UpdatePositionInput GetInputWithNameEmpty(Guid id)
            => new
            (
                id: id,
                customerId: Guid.NewGuid(),
                name: "",
                baseSalary: 10_000,
                loggedUsername: GetValidUsername()
            );

        public UpdatePositionInput GetInputWithCustomerIdEmpty(Guid id)
            => new
            (
                id: id,
                customerId: Guid.Empty,
                name: "",
                baseSalary: 10_000,
                loggedUsername: GetValidUsername()
            );

        public UpdatePositionInput GetInputWithUsernameEmpty(Guid id)
            => new
            (
                id: id,
                customerId: Guid.NewGuid(),
                name: GetValidName(),
                baseSalary: 10_000,
                loggedUsername: ""
            );
    }
}
