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
            => new()
            {
                Name = GetValidName(),
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public CreateCustomerInput GetCreateCustomerInputWithEmptyName()
            => new()
            {
                Name = "",
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };


        public CreateCustomerInput GetCreateCustomerInputWithEmptyDocument()
            => new()
            {
                Name = GetValidName(),
                Document = "",
                LoggedUsername = GetValidUsername()
            };

        public CreateCustomerInput GetCreateCustomerInputWithInvalidDocument()
            => new()
            {
                Name = GetValidName(),
                Document = GetInvalidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public CreateCustomerInput GetCreateCustomerInputWithEmptyLoggedUsername()
           => new()
           {
               Name = GetValidName(),
               Document = GetValidDocument(),
               LoggedUsername = ""
           };
    }


}
