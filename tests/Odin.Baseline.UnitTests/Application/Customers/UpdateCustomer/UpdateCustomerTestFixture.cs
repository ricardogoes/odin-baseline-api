using Odin.Baseline.Application.Customers.UpdateCustomer;
using Odin.Baseline.UnitTests.Application.Customers.Common;

namespace Odin.Baseline.UnitTests.Application.Customers.UpdateCustomer
{
    [CollectionDefinition(nameof(UpdateCustomerTestFixtureCollection))]
    public class UpdateCustomerTestFixtureCollection : ICollectionFixture<UpdateCustomerTestFixture>
    { }

    public class UpdateCustomerTestFixture : CustomerBaseFixture
    {
        public UpdateCustomerTestFixture()
            : base() { }

        public UpdateCustomerInput GetValidUpdateCustomerInput(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Name = GetValidName(),
                Document = GetValidDocument(),                
                LoggedUsername = GetValidUsername()
            };

        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyName(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Name = "",
                Document = GetValidDocument(),
                LoggedUsername = GetValidUsername()
            };


        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyDocument(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Name = GetValidName(),
                Document = "",
                LoggedUsername = GetValidUsername()
            };

        public UpdateCustomerInput GetUpdateCustomerInputWithInvalidDocument(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Name = GetValidName(),
                Document = GetInvalidDocument(),
                LoggedUsername = GetValidUsername()
            };

        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyLoggedUsername(Guid? id = null)
           => new()
           {
               Id = id ?? Guid.NewGuid(),
               Name = GetValidName(),
               Document = GetValidDocument(),
               LoggedUsername = ""
           };
    }


}
