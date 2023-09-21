using Bogus;
using Bogus.Extensions.Brazil;
using Keycloak.AuthServices.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.EndToEndTests.Api;
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

        protected BaseFixture()
        {
            Faker = new Faker("pt_BR");

            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();

            var configuration = WebAppFactory.Services.GetRequiredService<IConfiguration>();

            var keycloakOptions = configuration
                .GetSection(KeycloakAuthenticationOptions.Section)
                .Get<KeycloakAuthenticationOptions>();

            ApiClient = new ApiClient(HttpClient, keycloakOptions!);
            ArgumentNullException.ThrowIfNull(configuration);
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

        public string GetValidName()
           => Faker.Company.CompanyName(1);

        public string GetValidDocument()
            => Faker.Company.Cnpj();

        public string GetInvalidDocument()
           => "12.123.123/0002-12";

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidName(), GetValidDocument(), isActive: GetRandomBoolean());
            customer.Create("unit.testing");

            return customer;
        }

        public CustomerModel GetValidCustomerModel()
        {
            return new CustomerModel
            (
                id: Guid.NewGuid(),
                name: GetValidName(),
                document: GetValidDocument(),
                isActive: GetRandomBoolean(),
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );
        }

        public string GetValidDepartmentName()
           => Faker.Commerce.Department();

        public Department GetValidDepartment(Guid? id = null)
        {
            var department = new Department(id ?? Guid.NewGuid(), GetValidDepartmentName(), isActive: GetRandomBoolean());
            department.Create("unit.testing");

            return department;
        }

        public DepartmentModel GetValidDepartmentModel(Guid? customerId = null)
        {
            var customer = new DepartmentModel
            (
                id: Guid.NewGuid(),
                customerId: customerId ?? Guid.NewGuid(),
                name: GetValidDepartmentName(),
                isActive: GetRandomBoolean(),
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );

            return customer;
        }
    }
}
