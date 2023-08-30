using Bogus;
using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Infra.Data.EF;

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
    }
}
