namespace Odin.Baseline.UnitTests.Application.Employees.UpdateEmployee
{
    public class UpdateEmployeeTestDataGenerator
    {

        public static IEnumerable<object[]> GetEmployeesToUpdate(int times = 10)
        {
            var fixture = new UpdateEmployeeTestFixture();
            for (int indice = 0; indice < times; indice++)
            {
                var validEmployee = fixture.GetValidEmployee();
                var validInpur = fixture.GetValidUpdateEmployeeInput(validEmployee.Id);
                yield return new object[] {
                validEmployee, validInpur
            };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateEmployeeTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
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
