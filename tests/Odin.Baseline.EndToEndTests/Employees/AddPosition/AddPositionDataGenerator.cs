namespace Odin.Baseline.EndToEndTests.Employees.AddPosition
{
    public class AddPositionApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new AddPositionApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 12;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input2 = fixture.GetInputWithEmptyPositionId();
                        invalidInputsList.Add(new object[] {
                        input2,
                        "PositionId should not be empty or null"
                    });
                        break;
                    case 1:
                        var input3 = fixture.GetInputWithEmptySalary();
                        invalidInputsList.Add(new object[] {
                        input3,
                        "Salary should not be empty or null"
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
