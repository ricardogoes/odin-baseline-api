namespace Odin.Baseline.UnitTests.Application.Positions.UpdatePosition
{
    public class UpdatePositionTestDataGenerator
    {

        public static IEnumerable<object[]> GetPositionsToUpdate(int times = 10)
        {
            var fixture = new UpdatePositionTestFixture();
            for (int indice = 0; indice < times; indice++)
            {
                var validPosition = fixture.GetValidPosition();
                var validInpur = fixture.GetValidUpdatePositionInput(validPosition.Id);
                yield return new object[] {
                validPosition, validInpur
            };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdatePositionTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdatePositionInputWithEmptyCustomerId(),
                        "CustomerId should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdatePositionInputWithEmptyName(),
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
