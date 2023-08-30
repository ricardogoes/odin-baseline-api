using FluentAssertions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Mappers
{
    [Collection(nameof(ModelMapperTestFixtureCollection))]
    public class DepartmentModelMapperTest
    {
        private readonly ModelMapperTestFixture _fixture;

        public DepartmentModelMapperTest(ModelMapperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ToDepartmentModel() should map an Department to DepartmentModel")]
        [Trait("Infra.Data.EF", "Mappers / DepartmentModelMapper")]
        public void MapDepartmentToDepartmentModel()
        {
            var department = _fixture.GetValidDepartment();
            var model = department.ToDepartmentModel();

            model.Should().NotBeNull();
            model.Id.Should().Be(department.Id);
            model.CustomerId.Should().Be(department.CustomerId);
            model.Name.Should().Be(department.Name);
            model.IsActive.Should().Be(department.IsActive);
            model.CreatedAt.Should().Be(department.CreatedAt);
            model.CreatedBy.Should().Be(department.CreatedBy);
            model.LastUpdatedAt.Should().Be(department.LastUpdatedAt);
            model.LastUpdatedBy.Should().Be(department.LastUpdatedBy);
        }

        [Fact(DisplayName = "ToDepartmentModel() should map a list of departments to DepartmentModel")]
        [Trait("Infra.Data.EF", "Mappers / DepartmentModelMapper")]
        public void MapListDepartmentsToDepartmentModel()
        {
            var department1 = _fixture.GetValidDepartment();            
            var department2 = _fixture.GetValidDepartment();
            var departments = new List<Department> { department1, department2 };

            var model = departments.ToDepartmentModel();

            model.Should().NotBeNull();
            foreach (var department in model)
            {
                var departmentToCompare = departments.FirstOrDefault(x => x.Id == department.Id);
                department.Id.Should().Be(departmentToCompare!.Id);
                department.CustomerId.Should().Be(departmentToCompare.CustomerId);
                department.Name.Should().Be(departmentToCompare.Name);
                department.IsActive.Should().Be(departmentToCompare.IsActive);
                department.CreatedAt.Should().Be(departmentToCompare.CreatedAt);
                department.CreatedBy.Should().Be(departmentToCompare.CreatedBy);
                department.LastUpdatedAt.Should().Be(departmentToCompare.LastUpdatedAt);
                department.LastUpdatedBy.Should().Be(departmentToCompare.LastUpdatedBy);
            }
        }

        [Fact(DisplayName = "ToDepartment() should map an DepartmentModel to Department")]
        [Trait("Infra.Data.EF", "Mappers / DepartmentModelMapper")]
        public void MapDepartmentModelToDepartmentWithAddress()
        {
            var model = _fixture.GetValidDepartmentModel();
            var department = model.ToDepartment();

            department.Should().NotBeNull();
            department.Id.Should().Be(model.Id);
            department.CustomerId.Should().Be(model.CustomerId);
            department.Name.Should().Be(model.Name);
            department.IsActive.Should().Be(model.IsActive);
            department.CreatedAt.Should().Be(model.CreatedAt);
            department.CreatedBy.Should().Be(model.CreatedBy);
            department.LastUpdatedAt.Should().Be(model.LastUpdatedAt);
            department.LastUpdatedBy.Should().Be(model.LastUpdatedBy);
        }

        [Fact(DisplayName = "ToDepartment() should map a list of departments models to Department")]
        [Trait("Infra.Data.EF", "Mappers / DepartmentModelMapper")]
        public void MapListDepartmentsModelToDepartmen()
        {            
            var departmentModel1 = _fixture.GetValidDepartmentModel();
            var departmentModel2 = _fixture.GetValidDepartmentModel();
            var departmentsModel = new List<DepartmentModel> { departmentModel1, departmentModel2 };

            var departments = departmentsModel.ToDepartment();

            departments.Should().NotBeNull();
            foreach (var department in departments)
            {
                var departmentToCompare = departmentsModel.FirstOrDefault(x => x.Id == department.Id);
                department.Should().NotBeNull();
                department.Id.Should().Be(departmentToCompare!.Id);
                department.CustomerId.Should().Be(departmentToCompare.CustomerId);
                department.Name.Should().Be(departmentToCompare.Name);
                department.IsActive.Should().Be(departmentToCompare.IsActive);
                department.CreatedAt.Should().Be(departmentToCompare.CreatedAt);
                department.CreatedBy.Should().Be(departmentToCompare.CreatedBy);
                department.LastUpdatedAt.Should().Be(departmentToCompare.LastUpdatedAt);
                department.LastUpdatedBy.Should().Be(departmentToCompare.LastUpdatedBy);
            }
        }
    }
}
