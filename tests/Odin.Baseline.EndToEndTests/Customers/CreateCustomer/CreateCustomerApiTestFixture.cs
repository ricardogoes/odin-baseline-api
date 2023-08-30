using Odin.Baseline.Application.Customers.CreateCustomer;
using Odin.Baseline.EndToEndTests.Customers.Common;

namespace Odin.Baseline.EndToEndTests.Customers.CreateCustomer
{
    [CollectionDefinition(nameof(CreateCustomerApiTestCollection))]
    public class CreateCustomerApiTestCollection : ICollectionFixture<CreateCustomerApiTestFixture>
    { }

    public class CreateCustomerApiTestFixture : CustomerBaseFixture
    {
        public CreateCustomerApiTestFixture()
            : base()
        { }

        public CreateCustomerInput GetValidInput()
            => new()
            {
                Name = GetValidName(),
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public CreateCustomerInput GetInputWithNameEmpty()
            => new()
            {
                Name = "",
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public CreateCustomerInput GetInputWithDocumentEmpty()
            => new()
            {
                Name = GetValidName(),
                Document = "",
                LoggedUsername = GetValidUsername()
            };

        public CreateCustomerInput GetInputWithInvalidDocument()
            => new()
            {
                Name = GetValidName(),
                Document = GetInvalidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public CreateCustomerInput GetInputWithUsernameEmpty()
            => new()
            {
                Name = GetValidName(),
                Document = GetValidDocument(),
                LoggedUsername = ""
            };
    }
}
