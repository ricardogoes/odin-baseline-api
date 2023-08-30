namespace Odin.Baseline.EndToEndTests.Employees.UpdateEmployee
{
    public class UpdateEmployeeApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new UpdateEmployeeApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateEmployeeInputWithEmptyFirstName(),
                        "FirstName should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateEmployeeInputWithEmptyLastName(),
                        "LastName should not be empty or null"
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateEmployeeInputWithEmptyDocument(),
                        "Document should be a valid CPF or CNPJ"
                    });
                        break;
                    case 3:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateEmployeeInputWithInvalidDocument(),
                        "Document should be a valid CPF or CNPJ"
                    });
                        break;
                    case 4:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateEmployeeInputWithInvalidEmail(),
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
