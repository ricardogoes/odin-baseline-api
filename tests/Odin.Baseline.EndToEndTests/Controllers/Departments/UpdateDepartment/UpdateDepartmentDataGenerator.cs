﻿namespace Odin.Baseline.EndToEndTests.Controllers.Departments.UpdateDepartment
{
    public class UpdateDepartmentApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new UpdateDepartmentApiTestFixture();
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
