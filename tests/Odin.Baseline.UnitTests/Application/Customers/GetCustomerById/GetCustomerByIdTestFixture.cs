using Odin.Baseline.Application.Customers.GetCustomerById;
using Odin.Baseline.UnitTests.Application.Customers.Common;

namespace Odin.Baseline.UnitTests.Application.Customers.GetCustomerById
{
    [CollectionDefinition(nameof(GetCustomerByIdTestFixture))]
    public class GetCustomerByIdTestFixtureCollection :ICollectionFixture<GetCustomerByIdTestFixture>
    { }

    public class GetCustomerByIdTestFixture: CustomerBaseFixture
    {
        public GetCustomerByIdInput GetValidGetCustomerByIdInput(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid()
            };
    }
}
