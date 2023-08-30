using Odin.Baseline.Application.Employees.ChangeAddressEmployee;
using Odin.Baseline.UnitTests.Application.Employees.Common;

namespace Odin.Baseline.UnitTests.Application.Employees.ChangeAddressEmployee
{
    [CollectionDefinition(nameof(ChangeAddressEmployeeTestFixtureCollection))]
    public class ChangeAddressEmployeeTestFixtureCollection : ICollectionFixture<ChangeAddressEmployeeTestFixture>
    { }

    public class ChangeAddressEmployeeTestFixture : EmployeeBaseFixture
    {
        public ChangeAddressEmployeeTestFixture()
            : base() { }

        public ChangeAddressEmployeeInput GetValidInputAddress()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressEmployeeInput GetInputAddressWithEmployeeIdEmpty()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.Empty,
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressEmployeeInput GetInputAddressWithStreetNameEmpty()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.NewGuid(),
                StreetName = string.Empty,
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressEmployeeInput GetInputAddressWithStreetNumberEmpty()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = 0,
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressEmployeeInput GetInputAddressWithNeighborhoodEmpty()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = string.Empty,
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressEmployeeInput GetInputAddressWithZipCodeEmpty()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = string.Empty,
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressEmployeeInput GetInputAddressWithCityEmpty()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = string.Empty,
                State = Faker.Address.StateAbbr()
            };
            return address;
        }

        public ChangeAddressEmployeeInput GetInputAddressWithStateEmpty()
        {
            var address = new ChangeAddressEmployeeInput
            {
                EmployeeId = Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = string.Empty
            };
            return address;
        }
    }
}
