using Odin.Baseline.Application.Employees.ChangeAddressEmployee;
using Odin.Baseline.EndToEndTests.Controllers.Employees;

namespace Odin.Baseline.EndToEndTests.Controllers.Employees.ChangeAddressEmployee
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
            => new
            (
                employeeId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressEmployeeInput GetAddressInputWithoutStreetName(Guid? id = null)
            => new
            (
                employeeId: id ?? Guid.NewGuid(),
                streetName: "",
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressEmployeeInput GetAddressInputWithoutStreetNumber(Guid? id = null)
            => new
            (
                employeeId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: -1,
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressEmployeeInput GetAddressInputWithoutNeighborhood(Guid? id = null)
            => new
            (
                employeeId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: "",
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressEmployeeInput GetAddressInputWithoutZipCode(Guid? id = null)
            => new
            (
                employeeId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: "",
                city: Faker.Address.City(),
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressEmployeeInput GetAddressInputWithoutCity(Guid? id = null)
            => new
            (
               employeeId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: "",
                state: Faker.Address.StateAbbr()
            );

        public ChangeAddressEmployeeInput GetAddressInputWithoutState(Guid? id = null)
            => new
            (
                employeeId: id ?? Guid.NewGuid(),
                streetName: Faker.Address.StreetName(),
                streetNumber: int.Parse(Faker.Address.BuildingNumber()),
                complement: Faker.Address.SecondaryAddress(),
                neighborhood: Faker.Address.CardinalDirection(),
                zipCode: Faker.Address.ZipCode(),
                city: Faker.Address.City(),
                state: ""
            );
    }
}
