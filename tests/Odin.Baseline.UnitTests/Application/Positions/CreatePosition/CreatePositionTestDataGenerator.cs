namespace Odin.Baseline.UnitTests.Application.Positions.CreatePosition
{
    public class CreatePositionTestDataGenerator
    {


        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new CreatePositionTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreatePositionInputWithEmptyCustomerId(),
                        "CustomerId should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreatePositionInputWithEmptyName(),
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
