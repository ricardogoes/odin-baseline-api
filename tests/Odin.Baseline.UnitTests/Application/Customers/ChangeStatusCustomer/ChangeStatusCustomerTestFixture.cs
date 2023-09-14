using Odin.Baseline.Application.Customers.ChangeStatusCustomer;
using Odin.Baseline.UnitTests.Application.Customers.Common;

namespace Odin.Baseline.UnitTests.Application.Customers.ChangeStatusCustomer
{
    [CollectionDefinition(nameof(ChangeStatusCustomerTestFixtureCollection))]
    public class ChangeStatusCustomerTestFixtureCollection : ICollectionFixture<ChangeStatusCustomerTestFixture>
    { }

    public class ChangeStatusCustomerTestFixture : CustomerBaseFixture
    {
        public ChangeStatusCustomerTestFixture()
            : base() { }

        public ChangeStatusCustomerInput GetValidChangeStatusCustomerInputToActivate(Guid? id = null)
            => new
            (            
                id ?? Guid.NewGuid(),
                Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
                "unit.testing"
            );

        public ChangeStatusCustomerInput GetValidChangeStatusCustomerInputToDeactivate(Guid? id = null)
            => new
            (
                id ?? Guid.NewGuid(),
                Baseline.Domain.Enums.ChangeStatusAction.DEACTIVATE,
                "unit.testing"
            );

        public ChangeStatusCustomerInput GetChangeStatusCustomerInputWithEmptyAction(Guid? id = null)
          => new
          (
              id ?? Guid.NewGuid(),
              null,
              "unit.testing"
          );

        public ChangeStatusCustomerInput GetChangeStatusCustomerInputWithEmptyLoggedUsername(Guid? id = null)
           => new
           (
               id ?? Guid.NewGuid(),
               Baseline.Domain.Enums.ChangeStatusAction.ACTIVATE,
               ""
           );
    }
}
