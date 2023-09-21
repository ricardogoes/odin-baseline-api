using Odin.Baseline.Api.Models.Customers;
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
            => new
            (
                name: GetValidName(),
                document: GetValidDocument(),
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithNameEmpty()
            => new
            (
                name: "",
                document: GetValidDocument(),
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithDocumentEmpty()
            => new
            (
                name: GetValidName(),
                document: "",
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithInvalidDocument()
            => new
            (
                name: GetValidName(),
                document: GetInvalidDocument(),
                loggedUsername: GetValidUsername()
            );

        public CreateCustomerInput GetInputWithUsernameEmpty()
            => new
            (
                name: GetValidName(),
                document: GetValidDocument(),
                loggedUsername: ""
            );
    }
}
