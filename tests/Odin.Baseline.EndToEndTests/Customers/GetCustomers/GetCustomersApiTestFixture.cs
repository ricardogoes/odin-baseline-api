using Odin.Baseline.EndToEndTests.Customers.Common;
using Odin.Baseline.Infra.Data.EF.Models;

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
