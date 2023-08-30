using Odin.Baseline.Application.Employees.ChangeAddressEmployee;
using Odin.Baseline.EndToEndTests.Employees.Common;

namespace Odin.Baseline.EndToEndTests.Employees.ChangeAddressEmployee
{
    [CollectionDefinition(nameof(ChangeAddressEmployeeApiTestCollection))]
    public class ChangeAddressEmployeeApiTestCollection : ICollectionFixture<ChangeAddressEmployeeApiTestFixture>
    { }

    public class ChangeAddressEmployeeApiTestFixture : EmployeeBaseFixture
    {
        public ChangeAddressEmployeeApiTestFixture()
            : base()
        { }

        public ChangeAddressEmployeeInput GetValidInput(Guid? id = null)
            => new()
            {
                EmployeeId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressEmployeeInput GetAddressInputWithoutStreetName(Guid? id = null)
            => new()
            {
                EmployeeId = id ?? Guid.NewGuid(),
                StreetName = "",
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressEmployeeInput GetAddressInputWithoutStreetNumber(Guid? id = null)
            => new()
            {
                EmployeeId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = -1,
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressEmployeeInput GetAddressInputWithoutNeighborhood(Guid? id = null)
            => new()
            {
                EmployeeId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = "",
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressEmployeeInput GetAddressInputWithoutZipCode(Guid? id = null)
            => new()
            {
                EmployeeId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = "",
                City = Faker.Address.City(),
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressEmployeeInput GetAddressInputWithoutCity(Guid? id = null)
            => new()
            {
                EmployeeId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = "",
                State = Faker.Address.StateAbbr()
            };

        public ChangeAddressEmployeeInput GetAddressInputWithoutState(Guid? id = null)
            => new()
            {
                EmployeeId = id ?? Guid.NewGuid(),
                StreetName = Faker.Address.StreetName(),
                StreetNumber = int.Parse(Faker.Address.BuildingNumber()),
                Complement = Faker.Address.SecondaryAddress(),
                Neighborhood = Faker.Address.CardinalDirection(),
                ZipCode = Faker.Address.ZipCode(),
                City = Faker.Address.City(),
                State = ""
            };
    }
}
