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
            var position = new PositionModel
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId ?? Guid.NewGuid(),
                Name = GetValidName(),
                BaseSalary = 10_000,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return position;
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
