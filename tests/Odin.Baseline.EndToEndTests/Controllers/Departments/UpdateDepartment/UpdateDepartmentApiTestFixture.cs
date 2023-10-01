using Odin.Baseline.Application.Departments.UpdateDepartment;
using Odin.Baseline.EndToEndTests.Controllers.Departments;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments.UpdateDepartment
{
    [CollectionDefinition(nameof(UpdateDepartmentApiTestCollection))]
    public class UpdateDepartmentApiTestCollection : ICollectionFixture<UpdateDepartmentApiTestFixture>
    { }

    public class UpdateDepartmentApiTestFixture : DepartmentBaseFixture
    {
        public UpdateDepartmentApiTestFixture()
            : base()
        { }

        public UpdateDepartmentInput GetValidInput(Guid id)
            => new
            (
                id: id,
                name: GetValidDepartmentName()
            );

        public UpdateDepartmentInput GetInputWithIdEmpty()
            => new
            (
                id: Guid.Empty,
                name: GetValidDepartmentName()
            );


        public UpdateDepartmentInput GetInputWithNameEmpty(Guid id)
            => new
            (
                id: id,
                name: ""
            );
    }
}
