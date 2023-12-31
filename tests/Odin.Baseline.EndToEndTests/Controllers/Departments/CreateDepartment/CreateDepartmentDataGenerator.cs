﻿namespace Odin.Baseline.EndToEndTests.Controllers.Departments.CreateDepartment
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
                        "Name",
                        "'Name' must not be empty."
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
