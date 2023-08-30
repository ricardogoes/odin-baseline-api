using Odin.Baseline.Application.Customers.ChangeStatusCustomer;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.EndToEndTests.Customers.Common;

namespace Odin.Baseline.EndToEndTests.Customers.ChangeStatusCustomer
{
    [CollectionDefinition(nameof(ChangeStatusCustomerApiTestCollection))]
    public class ChangeStatusCustomerApiTestCollection : ICollectionFixture<ChangeStatusCustomerApiTestFixture>
    { }

    public class ChangeStatusCustomerApiTestFixture : CustomerBaseFixture
    {
        public ChangeStatusCustomerApiTestFixture()
            : base()
        { }

        public ChangeStatusCustomerInput GetValidInputToActivate(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.ACTIVATE,
                LoggedUsername = GetValidUsername()
            };

        public ChangeStatusCustomerInput GetValidInputToDeactivate(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.DEACTIVATE,
                LoggedUsername = GetValidUsername()
            };

        public ChangeStatusCustomerInput GetInputWithInvalidAction(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid(),
                Action = ChangeStatusAction.INVALID,
                LoggedUsername = GetValidUsername()
            };
    }
}
