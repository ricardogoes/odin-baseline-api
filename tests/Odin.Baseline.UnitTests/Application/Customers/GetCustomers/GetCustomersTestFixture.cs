using Odin.Baseline.UnitTests.Application.Customers.Common;

namespace Odin.Baseline.UnitTests.Application.Customers.GetCustomers
{
    [CollectionDefinition(nameof(GetCustomerByIdTestFixtureCollection))]
    public class GetCustomerByIdTestFixtureCollection : ICollectionFixture<GetCustomersTestFixture>
    { }

    public class GetCustomersTestFixture : CustomerBaseFixture
    { }
}
