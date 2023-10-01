using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models.AppSettings;
using Odin.Baseline.EndToEndTests.Configurations;
using Odin.Baseline.Infra.Data.EF;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.EndToEndTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public ApiClient ApiClient { get; set; }

        public Guid TenantSinapseId = Guid.Parse("5F9B7808-803F-4985-9996-6EBA9003F9CD");
        public Guid TenantMerxId = Guid.Parse("BEC7E4B4-2E23-4536-9B01-DC9E8D66ED5A");

        protected BaseFixture()
        {
            Faker = new Faker("pt_BR");

            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();

            var configuration = WebAppFactory.Services.GetRequiredService<IConfiguration>();
            ArgumentNullException.ThrowIfNull(configuration);

            var connectionStrings = new ConnectionStringsSettings(Environment.GetEnvironmentVariable("OdinSettings:ConnectionStrings:OdinMasterDB")!);

            var keycloakSettings = configuration.GetSection("Keycloak").Get<KeycloakSettings>()!;
            keycloakSettings.Credentials!.Secret = Environment.GetEnvironmentVariable("OdinSettings:Keycloak:Credentials:Secret")!;

            var appSettings = new AppSettings(connectionStrings, keycloakSettings);
            ApiClient = new ApiClient(HttpClient, appSettings!, TenantSinapseId);            
        }

        public OdinBaselineDbContext CreateDbContext(bool preserveData = false)
        {
            var tenantService = WebAppFactory.Services.GetRequiredService<ITenantService>();

            var context = new OdinBaselineDbContext(
                new DbContextOptionsBuilder<OdinBaselineDbContext>()
                .UseInMemoryDatabase("e2e-tests-db")
                .Options,
                tenantService
            );

            if (preserveData == false)
                context.Database.EnsureDeleted();

            return context;
        }

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";
                
        public string GetValidDepartmentName()
           => Faker.Commerce.Department();

        public Department GetValidDepartment(Guid? id = null)
        {
            var department = new Department(id ?? Guid.NewGuid(), GetValidDepartmentName(), isActive: GetRandomBoolean());
            return department;
        }

        public DepartmentModel GetValidDepartmentModel(Guid? customerId = null)
        {
            var department = new DepartmentModel
            (
                id: Guid.NewGuid(),
                name: GetValidDepartmentName(),
                isActive: GetRandomBoolean(),
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test",
                tenantId: TenantSinapseId
            );

            return department;
        }
    }
}
