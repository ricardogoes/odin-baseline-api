namespace Odin.Baseline.UnitTests.Application.Departments.UpdateDepartment
{
    public class UpdateDepartmentTestDataGenerator
    {

        public static IEnumerable<object[]> GetDepartmentsToUpdate(int times = 10)
        {
            var fixture = new UpdateDepartmentTestFixture();
            for (int indice = 0; indice < times; indice++)
            {
                var validDepartment = fixture.GetValidDepartment();
                var validInpur = fixture.GetValidUpdateDepartmentInput(validDepartment.Id);
                yield return new object[] {
                validDepartment, validInpur
            };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateDepartmentTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateDepartmentInputWithEmptyCustomerId(),
                        "CustomerId should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateDepartmentInputWithEmptyName(),
                        "Name should not be empty or null"
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
