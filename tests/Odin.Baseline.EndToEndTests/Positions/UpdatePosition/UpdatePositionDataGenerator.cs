namespace Odin.Baseline.EndToEndTests.Positions.UpdatePosition
{
    public class UpdatePositionApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new UpdatePositionApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input1 = fixture.GetInputWithNameEmpty(Guid.NewGuid());
                        invalidInputsList.Add(new object[] {
                        input1,
                        "Name should not be empty or null"
                    });
                        break;
                    case 1:
                        var input2 = fixture.GetInputWithCustomerIdEmpty(Guid.NewGuid());
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
