namespace Odin.Baseline.EndToEndTests.Employees.ChangeAddressEmployee
{
    public class ChangeAddressEmployeeApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new ChangeAddressEmployeeApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 12;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input1 = fixture.GetAddressInputWithoutStreetName();
                        invalidInputsList.Add(new object[] {
                        input1,
                        "StreetName should not be empty or null"
                    });
                        break;
                    case 1:
                        var input2 = fixture.GetAddressInputWithoutStreetNumber();
                        invalidInputsList.Add(new object[] {
                        input2,
                        "StreetNumber should not be empty or null"
                    });
                        break;
                    case 2:
                        var input3 = fixture.GetAddressInputWithoutNeighborhood();
                        invalidInputsList.Add(new object[] {
                        input3,
                        "Neighborhood should not be empty or null"
                    });
                        break;
                    case 3:
                        var input4 = fixture.GetAddressInputWithoutZipCode();
                        invalidInputsList.Add(new object[] {
                        input4,
                        "ZipCode should not be empty or null"
                    });
                        break;
                    case 4:
                        var input5 = fixture.GetAddressInputWithoutCity();
                        invalidInputsList.Add(new object[] {
                        input5,
                        "City should not be empty or null"
                    });
                        break;
                    case 5:
                        var input6 = fixture.GetAddressInputWithoutState();
                        invalidInputsList.Add(new object[] {
                        input6,
                        "State should not be empty or null"
                    });
                        break;
                    default:
                        break;
                }
            }

            return invalidInputsList;
        }
    }
}
