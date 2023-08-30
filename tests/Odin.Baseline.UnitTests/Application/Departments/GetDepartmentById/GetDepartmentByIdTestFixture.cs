﻿using Odin.Baseline.Application.Departments.GetDepartmentById;
using Odin.Baseline.UnitTests.Application.Departments.Common;

namespace Odin.Baseline.UnitTests.Application.Departments.GetDepartmentById
{
    [CollectionDefinition(nameof(GetDepartmentByIdTestFixture))]
    public class GetDepartmentByIdTestFixtureCollection :ICollectionFixture<GetDepartmentByIdTestFixture>
    { }

    public class GetDepartmentByIdTestFixture: DepartmentBaseFixture
    {
        public GetDepartmentByIdInput GetValidGetDepartmentByIdInput(Guid? id = null)
            => new()
            {
                Id = id ?? Guid.NewGuid()
            };
    }
}
