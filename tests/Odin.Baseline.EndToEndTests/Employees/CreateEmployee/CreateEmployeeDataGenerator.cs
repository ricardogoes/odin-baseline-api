namespace Odin.Baseline.EndToEndTests.Employees.CreateEmployee
{
    public class CreateEmployeeApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateEmployeeApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithEmptyFirstName(),
                        "FirstName should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithEmptyLastName(),
                        "LastName should not be empty or null"
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithEmptyDocument(),
                        "Document should be a valid CPF or CNPJ"
                    });
                        break;
                    case 3:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithInvalidDocument(),
                        "Document should be a valid CPF or CNPJ"
                    });
                        break;
                    case 4:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithInvalidEmail(),
                        "Email should be a valid email"
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
