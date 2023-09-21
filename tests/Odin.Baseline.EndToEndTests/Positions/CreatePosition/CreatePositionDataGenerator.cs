﻿namespace Odin.Baseline.EndToEndTests.Positions.CreatePosition
{
    public class CreatePositionApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreatePositionApiTestFixture();
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
                        "Name",
                        "'Name' must not be empty."
                    });
                        break;
                    case 1:
                        var input2 = fixture.GetInputWithCustomerIdEmpty();
                        invalidInputsList.Add(new object[] {
                        input2,
                        "CustomerId",
                        "'Customer Id' must not be empty."
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
