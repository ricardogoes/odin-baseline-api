﻿using Odin.Baseline.Application.Employees.GetEmployeeById;

namespace Odin.Baseline.UnitTests.Application.Employees.GetEmployeeById
{
    [CollectionDefinition(nameof(GetEmployeeByIdTestFixture))]
    public class GetEmployeeByIdTestFixtureCollection :ICollectionFixture<GetEmployeeByIdTestFixture>
    { }

    public class GetEmployeeByIdTestFixture: EmployeeBaseFixture
    {
        public GetEmployeeByIdInput GetValidGetEmployeeByIdInput(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid()
            };
    }
}
