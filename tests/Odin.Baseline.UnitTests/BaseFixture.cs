using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.EntityFrameworkCore;
using Moq;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.ValueObjects;
using Odin.Baseline.Infra.Data.EF;

namespace Odin.Baseline.UnitTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }
        public Guid TenantId = Guid.NewGuid();

        protected BaseFixture()
            => Faker = new Faker("pt_BR");

        public OdinBaselineDbContext CreateDbContext(bool preserveData = false)
        {
            var tenantServiceMock = new Mock<ITenantService>();
            tenantServiceMock.Setup(s => s.GetTenant()).Returns(TenantId);

            var context = new OdinBaselineDbContext(
                new DbContextOptionsBuilder<OdinBaselineDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options,
                tenantServiceMock.Object
            );

            if (preserveData == false)
                context.Database.EnsureDeleted();
            return context;
        }

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public static string GetInvalidUsersername()
            => "";

        public static bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public Address GetValidAddress()
        {
            var address = new Address(
                Faker.Address.StreetName(),
                int.Parse(Faker.Address.BuildingNumber()),
                Faker.Address.SecondaryAddress(),
                Faker.Address.CardinalDirection(),
                Faker.Address.ZipCode(),
                Faker.Address.City(),
                Faker.Address.StateAbbr()
            );
            return address;
        }

        public string GetValidDepartmentName()
            => Faker.Company.CompanyName(1);

        public string GetValidDepartmentDocument()
            => Faker.Company.Cnpj();

        public Department GetValidDepartment()
        {
            var department = new Department(GetValidDepartmentName());

            return department;
        }

        public Department GetValidDepartmentWithId()
        {
            var department = new Department(Guid.NewGuid(), GetValidDepartmentName());

            return department;
        }
    }
}

