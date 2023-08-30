namespace Odin.Baseline.UnitTests.Application.Departments.CreateDepartment
{
    public class CreateDepartmentTestDataGenerator
    {


        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new CreateDepartmentTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateDepartmentInputWithEmptyCustomerId(),
                        "CustomerId should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateDepartmentInputWithEmptyName(),
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
