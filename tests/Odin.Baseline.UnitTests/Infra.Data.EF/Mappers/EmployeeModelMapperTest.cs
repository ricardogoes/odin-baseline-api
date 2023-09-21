using FluentAssertions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Mappers
{
    [Collection(nameof(ModelMapperTestFixtureCollection))]
    public class EmployeeModelMapperTest
    {
        private readonly ModelMapperTestFixture _fixture;

        public EmployeeModelMapperTest(ModelMapperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ToEmployeeModel() should map an Employee to EmployeeModel without address")]
        [Trait("Infra.Data.EF", "Mappers / EmployeeModelMapper")]
        public void MapEmployeeToEmployeeModel()
        {
            var employee = _fixture.GetValidEmployee();
            var model = employee.ToEmployeeModel();

            model.Should().NotBeNull();
            model.Id.Should().Be(employee.Id);
            model.FirstName.Should().Be(employee.FirstName);
            model.LastName.Should().Be(employee.LastName);
            model.Document.Should().Be(employee.Document);
            model.Email.Should().Be(employee.Email);
            model.IsActive.Should().Be(employee.IsActive);
            model.CreatedAt.Should().Be(employee.CreatedAt);
            model.CreatedBy.Should().Be(employee.CreatedBy);
            model.LastUpdatedAt.Should().Be(employee.LastUpdatedAt);
            model.LastUpdatedBy.Should().Be(employee.LastUpdatedBy);

            model.StreetName.Should().BeNull();
            model.StreetNumber.Should().BeNull();
            model.Complement.Should().BeNull();
            model.Neighborhood.Should().BeNull();
            model.ZipCode.Should().BeNull();
            model.City.Should().BeNull();
            model.State.Should().BeNull();
        }

        [Fact(DisplayName = "ToEmployeeModel() should map an Employee to EmployeeModel with address")]
        [Trait("Infra.Data.EF", "Mappers / EmployeeModelMapper")]
        public void MapEmployeeToEmployeeModelWithAddress()
        {
            var employee = _fixture.GetValidEmployee();
            var address = _fixture.GetValidAddress();
            employee.ChangeAddress(address, "unit.testing");

            var model = employee.ToEmployeeModel();

            model.Should().NotBeNull();
            model.Id.Should().Be(employee.Id);
            model.FirstName.Should().Be(employee.FirstName);
            model.LastName.Should().Be(employee.LastName);
            model.Document.Should().Be(employee.Document);
            model.Email.Should().Be(employee.Email);
            model.IsActive.Should().Be(employee.IsActive);
            model.CreatedAt.Should().Be(employee.CreatedAt);
            model.CreatedBy.Should().Be(employee.CreatedBy);
            model.LastUpdatedAt.Should().Be(employee.LastUpdatedAt);
            model.LastUpdatedBy.Should().Be(employee.LastUpdatedBy);

            model.StreetName.Should().Be(employee.Address!.StreetName);
            model.StreetNumber.Should().Be(employee.Address.StreetNumber);
            model.Complement.Should().Be(employee.Address.Complement);
            model.Neighborhood.Should().Be(employee.Address.Neighborhood);
            model.ZipCode.Should().Be(employee.Address.ZipCode);
            model.City.Should().Be(employee.Address.City);
            model.State.Should().Be(employee.Address.State);
        }

        [Fact(DisplayName = "ToEmployeeModel() should map a list of employees to EmployeeModel with address")]
        [Trait("Infra.Data.EF", "Mappers / EmployeeModelMapper")]
        public void MapListEmployeesToEmployeeModelWithAddress()
        {
            var address = _fixture.GetValidAddress();

            var employee1 = _fixture.GetValidEmployee();            
            employee1.ChangeAddress(address, "unit.testing");

            var employee2 = _fixture.GetValidEmployee();
            employee2.ChangeAddress(address, "unit.testing");

            var employees = new List<Employee> { employee1, employee2 };

            var model = employees.ToEmployeeModel();

            model.Should().NotBeNull();
            foreach (var employee in model)
            {
                var employeeToCompare = employees.FirstOrDefault(x => x.Id == employee.Id);
                employee.Id.Should().Be(employeeToCompare!.Id);
                employee.FirstName.Should().Be(employeeToCompare.FirstName);
                employee.LastName.Should().Be(employeeToCompare.LastName);
                employee.Document.Should().Be(employeeToCompare.Document);
                employee.Email.Should().Be(employeeToCompare.Email);
                employee.IsActive.Should().Be(employeeToCompare.IsActive);
                employee.CreatedAt.Should().Be(employeeToCompare.CreatedAt);
                employee.CreatedBy.Should().Be(employeeToCompare.CreatedBy);
                employee.LastUpdatedAt.Should().Be(employeeToCompare.LastUpdatedAt);
                employee.LastUpdatedBy.Should().Be(employeeToCompare.LastUpdatedBy);

                employee.StreetName.Should().Be(employeeToCompare.Address!.StreetName);
                employee.StreetNumber.Should().Be(employeeToCompare.Address.StreetNumber);
                employee.Complement.Should().Be(employeeToCompare.Address.Complement);
                employee.Neighborhood.Should().Be(employeeToCompare.Address.Neighborhood);
                employee.ZipCode.Should().Be(employeeToCompare.Address.ZipCode);
                employee.City.Should().Be(employeeToCompare.Address.City);
                employee.State.Should().Be(employeeToCompare.Address.State);
            }
        }







        [Fact(DisplayName = "ToEmployee() should map an EmployeeModel to Employee with address")]
        [Trait("Infra.Data.EF", "Mappers / EmployeeModelMapper")]
        public void MapEmployeeModelToEmployeeWithAddress()
        {
            var model = _fixture.GetValidEmployeeModel();
            var employee = model.ToEmployee();

            employee.Should().NotBeNull();
            employee.Id.Should().Be(model.Id);
            employee.FirstName.Should().Be(model.FirstName);
            employee.LastName.Should().Be(model.LastName);
            employee.Document.Should().Be(model.Document);
            employee.Email.Should().Be(model.Email);
            employee.IsActive.Should().Be(model.IsActive);
            employee.CreatedAt.Should().Be(model.CreatedAt);
            employee.CreatedBy.Should().Be(model.CreatedBy);
            employee.LastUpdatedAt.Should().Be(model.LastUpdatedAt);
            employee.LastUpdatedBy.Should().Be(model.LastUpdatedBy);

            employee.Address.Should().NotBeNull();
            employee.Address!.StreetName.Should().Be(employee.Address.StreetName);
            employee.Address.StreetNumber.Should().Be(employee.Address.StreetNumber);
            employee.Address.Complement.Should().Be(employee.Address.Complement);
            employee.Address.Neighborhood.Should().Be(employee.Address.Neighborhood);
            employee.Address.ZipCode.Should().Be(employee.Address.ZipCode);
            employee.Address.City.Should().Be(employee.Address.City);
            employee.Address.State.Should().Be(employee.Address.State);
        }

        [Fact(DisplayName = "ToEmployee() should map an EmployeeModel to Employee without address")]
        [Trait("Infra.Data.EF", "Mappers / EmployeeModelMapper")]
        public void MapEmployeeModelToEmployeeWithoutAddress()
        {
            var model = _fixture.GetValidEmployeeModelWithoutAddress();
            var employee = model.ToEmployee();

            employee.Should().NotBeNull();
            employee.Id.Should().Be(model.Id);
            employee.FirstName.Should().Be(model.FirstName);
            employee.LastName.Should().Be(model.LastName);
            employee.Document.Should().Be(model.Document);
            employee.Email.Should().Be(model.Email);
            employee.Document.Should().Be(model.Document);
            employee.IsActive.Should().Be(model.IsActive);
            employee.CreatedAt.Should().Be(model.CreatedAt);
            employee.CreatedBy.Should().Be(model.CreatedBy);
            employee.LastUpdatedAt.Should().Be(model.LastUpdatedAt);
            employee.LastUpdatedBy.Should().Be(model.LastUpdatedBy);

            employee.Address.Should().BeNull();
        }

        [Fact(DisplayName = "ToEmployee() should map a list of employees models to Employee with address")]
        [Trait("Infra.Data.EF", "Mappers / EmployeeModelMapper")]
        public void MapListEmployeesModelToEmployeeWithAddress()
        {            
            var employeeModel1 = _fixture.GetValidEmployeeModel();
            var employeeModel2 = _fixture.GetValidEmployeeModel();

            var employeesModel = new List<EmployeeModel> { employeeModel1, employeeModel2 };

            var employees = employeesModel.ToEmployee();

            employees.Should().NotBeNull();
            foreach (var employee in employees)
            {
                var employeeToCompare = employeesModel.FirstOrDefault(x => x.Id == employee.Id);
                employee.Should().NotBeNull();
                employee.Id.Should().Be(employeeToCompare!.Id);
                employee.FirstName.Should().Be(employeeToCompare.FirstName);
                employee.LastName.Should().Be(employeeToCompare.LastName);
                employee.Document.Should().Be(employeeToCompare.Document);
                employee.Email.Should().Be(employeeToCompare.Email);
                employee.Document.Should().Be(employeeToCompare.Document);
                employee.IsActive.Should().Be(employeeToCompare.IsActive);
                employee.CreatedAt.Should().Be(employeeToCompare.CreatedAt);
                employee.CreatedBy.Should().Be(employeeToCompare.CreatedBy);
                employee.LastUpdatedAt.Should().Be(employeeToCompare.LastUpdatedAt);
                employee.LastUpdatedBy.Should().Be(employeeToCompare.LastUpdatedBy);

                employee.Address.Should().NotBeNull();
                employee.Address!.StreetName.Should().Be(employeeToCompare.StreetName);
                employee.Address.StreetNumber.Should().Be(employeeToCompare.StreetNumber);
                employee.Address.Complement.Should().Be(employeeToCompare.Complement);
                employee.Address.Neighborhood.Should().Be(employeeToCompare.Neighborhood);
                employee.Address.ZipCode.Should().Be(employeeToCompare.ZipCode);
                employee.Address.City.Should().Be(employeeToCompare.City);
                employee.Address.State.Should().Be(employeeToCompare.State);
            }
        }
    }
}
