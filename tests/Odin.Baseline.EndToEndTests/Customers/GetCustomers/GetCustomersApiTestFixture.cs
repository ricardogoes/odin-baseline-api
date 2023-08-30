using Odin.Baseline.Domain.Entities;
using Odin.Baseline.EndToEndTests.Customers.Common;

namespace Odin.Baseline.EndToEndTests.Customers.GetCustomers
{
    [CollectionDefinition(nameof(GetCustomersApiTestCollection))]
    public class GetCustomersApiTestCollection : ICollectionFixture<GetCustomersApiTestFixture>
    { }

    public class GetCustomersApiTestFixture : CustomerBaseFixture
    {
        public GetCustomersApiTestFixture()
            : base()
        { }
    }
}
