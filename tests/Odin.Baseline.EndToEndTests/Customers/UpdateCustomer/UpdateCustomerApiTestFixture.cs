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
            => new()
            {
                Id = id,
                Name = GetValidName(),
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateCustomerInput GetInputWithIdEmpty()
            => new()
            {
                Id = Guid.Empty,
                Name = GetValidName(),
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };


        public UpdateCustomerInput GetInputWithNameEmpty(Guid id)
            => new()
            {
                Id = id,
                Name = "",
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateCustomerInput GetInputWithDocumentEmpty(Guid id)
            => new()
            {
                Id = id,
                Name = GetValidName(),
                Document = "",
                LoggedUsername = GetValidUsername()
            };

        public UpdateCustomerInput GetInputWithInvalidDocument(Guid id)
            => new()
            {
                Id = id,
                Name = GetValidName(),
                Document = GetInvalidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateCustomerInput GetInputWithUsernameEmpty(Guid id)
            => new()
            {
                Id = id,
                Name = GetValidName(),
                Document = GetValidDocument(),
                LoggedUsername = ""
            };
    }
}
