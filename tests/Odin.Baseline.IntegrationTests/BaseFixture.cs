using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.IntegrationTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }

        protected BaseFixture()
            => Faker = new Faker("pt_BR");

        public OdinBaselineDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new OdinBaselineDbContext(
                new DbContextOptionsBuilder<OdinBaselineDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options
            );

            if (preserveData == false)
                context.Database.EnsureDeleted();
            return context;
        }

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public string GetValidName()
            => Faker.Company.CompanyName(1);

        public string GetValidDocument()
            => Faker.Company.Cnpj();

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidName(), GetValidDocument(), isActive: true);
            customer.Create("unit.testing");

            return customer;
        }

        public CustomerModel GetValidCustomerModel()
        {
            var customer = new CustomerModel
            {
                Id = Guid.NewGuid(),
                Name = GetValidName(),
                Document = GetValidDocument(),
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "unit.test",
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = "unit.test"
            };

            return customer;
        }
    }
}
