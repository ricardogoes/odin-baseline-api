using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odin.Baseline.EndToEndTests.Api;
using Odin.Baseline.Infra.Data.EF;

namespace Odin.Baseline.EndToEndTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }

        public ApiClient ApiClient { get; set; }

        protected BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CustomWebApplicationFactory<Program>();            
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
        }

        public OdinBaselineDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new OdinBaselineDbContext(
                new DbContextOptionsBuilder<OdinBaselineDbContext>()
                .UseInMemoryDatabase("e2e-tests-db")
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
