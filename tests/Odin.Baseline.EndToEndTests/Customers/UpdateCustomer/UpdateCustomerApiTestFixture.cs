using Odin.Baseline.Api.Models.Customers;
using Odin.Baseline.Application.Customers.UpdateCustomer;
using Odin.Baseline.EndToEndTests.Customers.Common;

namespace Odin.Baseline.EndToEndTests.Customers.UpdateCustomer
{
    [CollectionDefinition(nameof(UpdateCustomerApiTestCollection))]
    public class UpdateCustomerApiTestCollection : ICollectionFixture<UpdateCustomerApiTestFixture>
    { }

    public class UpdateCustomerApiTestFixture : CustomerBaseFixture
    {
        public UpdateCustomerApiTestFixture()
            : base()
        { }

        public UpdateCustomerInput GetValidInput(Guid id)
            => new
            (
                id: id,
                name: GetValidName(),
                document: GetValidDocument(),
                loggedUsername:GetValidUsername()
            );

        public UpdateCustomerInput GetInputWithIdEmpty()
            => new
            (
                id: Guid.Empty,
                name: GetValidName(),
                document: GetValidDocument(),
                loggedUsername:GetValidUsername()
            );


        public UpdateCustomerInput GetInputWithNameEmpty(Guid id)
            => new
            (
                id: id,
                name: "",
                document: GetValidDocument(),
                loggedUsername:GetValidUsername()
            );

        public UpdateCustomerInput GetInputWithDocumentEmpty(Guid id)
            => new
            (
                id: id,
                name: GetValidName(),
                document: "",
                loggedUsername:GetValidUsername()
            );

        public UpdateCustomerInput GetInputWithInvalidDocument(Guid id)
            => new
            (
                id: id,
                name: GetValidName(),
                document: GetInvalidDocument(),
                loggedUsername:GetValidUsername()
            );

        public UpdateCustomerInput GetInputWithUsernameEmpty(Guid id)
            => new
            (
                id: id,
                name: GetValidName(),
                document: GetValidDocument(),
                loggedUsername:""
            );
    }
}
