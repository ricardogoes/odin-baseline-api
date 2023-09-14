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
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city:Faker.Address.City(),
                state:Faker.Address.StateAbbr()
            );
        }

        public ChangeAddressEmployeeInput GetInputAddressWithEmployeeIdEmpty()
        {
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.Empty,
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city:Faker.Address.City(),
                state:Faker.Address.StateAbbr()
            );
        }

        public ChangeAddressEmployeeInput GetInputAddressWithStreetNameEmpty()
        {
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.NewGuid(),
                streetName: string.Empty,
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city:Faker.Address.City(),
                state:Faker.Address.StateAbbr()
            );
        }

        public ChangeAddressEmployeeInput GetInputAddressWithStreetNumberEmpty()
        {
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: 0,
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city:Faker.Address.City(),
                state:Faker.Address.StateAbbr()
            );
        }

        public ChangeAddressEmployeeInput GetInputAddressWithNeighborhoodEmpty()
        {
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: string.Empty,
                zipCode: Faker.Address.ZipCode(),
                city:Faker.Address.City(),
                state:Faker.Address.StateAbbr()
            );
        }

        public ChangeAddressEmployeeInput GetInputAddressWithZipCodeEmpty()
        {
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: string.Empty,
                city:Faker.Address.City(),
                state:Faker.Address.StateAbbr()
            );
        }

        public ChangeAddressEmployeeInput GetInputAddressWithCityEmpty()
        {
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city:string.Empty,
                state:Faker.Address.StateAbbr()
            );
        }

        public ChangeAddressEmployeeInput GetInputAddressWithStateEmpty()
        {
            return new ChangeAddressEmployeeInput
            (
                employeeId: Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city:Faker.Address.City(),
                state:string.Empty
            );
        }
    }
}
