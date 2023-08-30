namespace Odin.Baseline.EndToEndTests.Departments.CreateDepartment
{
    public class CreateDepartmentApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateDepartmentApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input1 = fixture.GetInputWithNameEmpty();
                        invalidInputsList.Add(new object[] {
                        input1,
                        "Name should not be empty or null"
                    });
                        break;
                    case 1:
                        var input2 = fixture.GetInputWithCustomerIdEmpty();
                        invalidInputsList.Add(new object[] {
                        input2,
                        "CustomerId should not be empty or null"
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
