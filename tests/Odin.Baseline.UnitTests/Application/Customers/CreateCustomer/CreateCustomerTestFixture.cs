using Odin.Baseline.Application.Customers.CreateCustomer;
using Odin.Baseline.UnitTests.Application.Customers.Common;

namespace Odin.Baseline.UnitTests.Application.Customers.CreateCustomer
{
    [CollectionDefinition(nameof(CreateCustomerTestFixtureCollection))]
    public class CreateCustomerTestFixtureCollection : ICollectionFixture<CreateCustomerTestFixture>
    { }

    public class CreateCustomerTestFixture : CustomerBaseFixture
    {
        public CreateCustomerTestFixture()
            : base() { }

        public CreateCustomerInput GetValidCreateCustomerInput()
            => new
            (
                GetValidCustomerName(),
                GetValidCustomerDocument(),
                GetValidUsername()
            );

        public CreateCustomerInput GetCreateCustomerInputWithEmptyName()
            => new
            (
                "",
                GetValidCustomerDocument(),
                GetValidUsername()
            );


        public CreateCustomerInput GetCreateCustomerInputWithEmptyDocument()
            => new
            (
                GetValidCustomerName(),
                "",
                GetValidUsername()
            );

        public CreateCustomerInput GetCreateCustomerInputWithInvalidDocument()
            => new
            (
                GetValidCustomerName(),
                "12.132.432/0002-12",
                GetValidUsername()
            );

        public CreateCustomerInput GetCreateCustomerInputWithEmptyLoggedUsername()
           => new
           (
               GetValidCustomerName(),
               GetValidCustomerDocument(),
               ""
           );
    }


}
