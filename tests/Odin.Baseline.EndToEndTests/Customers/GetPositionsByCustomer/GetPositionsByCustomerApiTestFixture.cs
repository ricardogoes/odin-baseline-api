using Odin.Baseline.EndToEndTests.Customers.Common;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests.Customers.GetPositionsByCustomer
{
    [CollectionDefinition(nameof(GetPositionsByCustomerApiTestCollection))]
    public class GetPositionsByCustomerApiTestCollection : ICollectionFixture<GetPositionsByCustomerApiTestFixture>
    { }

    public class GetPositionsByCustomerApiTestFixture : CustomerBaseFixture
    {
        public GetPositionsByCustomerApiTestFixture()
            : base()
        { }

        public PositionModel GetValidPositionModel(Guid? customerId = null)
        {
            return new PositionModel
            (
                id: Guid.NewGuid(),
                customerId: customerId ?? Guid.NewGuid(),
                name: GetValidName(),
                baseSalary: 10_000,
                isActive: true,
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );
        }

        public List<PositionModel> GetValidPositionsModelList(List<Guid> customersIds, int length = 10)
        {
            var positions = new List<PositionModel>();
            customersIds.ForEach(customerId =>
            {
                positions.AddRange(Enumerable.Range(1, length).Select(_ => GetValidPositionModel(customerId)).ToList());
            });

            return positions;
        }
    }
}
