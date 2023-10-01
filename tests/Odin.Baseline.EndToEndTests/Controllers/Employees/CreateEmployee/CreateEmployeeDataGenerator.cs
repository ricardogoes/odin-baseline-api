namespace Odin.Baseline.EndToEndTests.Controllers.Employees.CreateEmployee
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
                        "FirstName",
                        "'First Name' must not be empty."
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithEmptyLastName(),
                        "LastName",
                        "'Last Name' must not be empty."
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithEmptyDocument(),
                        "Document",
                        "'Document' must be a valid CPF or CNPJ"
                    });
                        break;
                    case 3:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithInvalidDocument(),
                        "Document",
                        "'Document' must be a valid CPF or CNPJ"
                    });
                        break;
                    case 4:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateEmployeeInputWithInvalidEmail(),
                        "Email",
                        "'Email' must be a valid email address."
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
