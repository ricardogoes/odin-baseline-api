using Odin.Baseline.EndToEndTests.Customers.Common;

namespace Odin.Baseline.EndToEndTests.Customers.GetCustomerById
{
    [CollectionDefinition(nameof(GetCustomerByIdApiTestCollection))]
    public class GetCustomerByIdApiTestCollection : ICollectionFixture<GetCustomerByIdApiTestFixture>
    { }

    public class GetCustomerByIdApiTestFixture : CustomerBaseFixture
    {
        public GetCustomerByIdApiTestFixture()
            : base()
        { }
    }
}
