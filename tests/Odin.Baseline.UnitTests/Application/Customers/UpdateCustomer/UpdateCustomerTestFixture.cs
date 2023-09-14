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
            => new
            (
                id ?? Guid.NewGuid(),
                GetValidCustomerName(),
                GetValidCustomerDocument(),                
                GetValidUsername()
            );

        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyName(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                "",
                GetValidCustomerDocument(),
                GetValidUsername()
            );


        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyDocument(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                GetValidCustomerName(),
                "",
                GetValidUsername()
            );

        public UpdateCustomerInput GetUpdateCustomerInputWithInvalidDocument(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                GetValidCustomerName(),
                "12.312.534/0002-12",
                GetValidUsername()
            );

        public UpdateCustomerInput GetUpdateCustomerInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               GetValidCustomerName(),
               GetValidCustomerDocument(),
               ""
           );
    }
}
