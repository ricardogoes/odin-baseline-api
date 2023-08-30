using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Employees.GetEmployees;

namespace Odin.Baseline.UnitTests.Application.Employees.GetEmployees
{
    [Collection(nameof(GetEmployeesTestFixtureCollection))]
    public class GetEmployeesTest
    {
        private readonly GetEmployeesTestFixture _fixture;

        private readonly Mock<IEmployeeRepository> _repositoryMock;

        public GetEmployeesTest(GetEmployeesTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should return data filtered")]
        [Trait("Application", "Employees / GetEmployees")]
        public async Task GetEmployees()
        {
            var expectedEmployees = new PaginatedListOutput<Employee>
            {
                TotalItems = 15,
                Items = new List<Employee>
                {
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                }
            };

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedEmployees));

            var useCase = new App.GetEmployees(_repositoryMock.Object);
            var employees = await useCase.Handle(new App.GetEmployeesInput(1, 10, "name", Guid.Empty, Guid.Empty, "", "", "", "", true), new CancellationToken());

            employees.Should().NotBeNull();
            employees.TotalItems.Should().Be(15);
            employees.TotalPages.Should().Be(2);
            employees.PageNumber.Should().Be(1);
            employees.PageSize.Should().Be(10);
            employees.Items.Should().HaveCount(15);
        }

        [Fact(DisplayName = "Handle() should return 1 page when total items is less than page size")]
        [Trait("Application", "Employees / GetEmployees")]
        public async Task GetOnePageWhenTotalItemsLessPageSize()
        {
            var expectedEmployees = new PaginatedListOutput<Employee>
            {
                TotalItems = 4,
                Items = new List<Employee>
                {
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                    new Employee(Guid.NewGuid(), _fixture.GetValidFirstName(), _fixture.GetValidLastName(), _fixture.GetValidDocument(), _fixture.GetValidEmail(), Guid.NewGuid(), isActive: true),
                }
            };

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedEmployees));

            var useCase = new App.GetEmployees(_repositoryMock.Object);
            var employees = await useCase.Handle(new App.GetEmployeesInput(1, 10, "name", Guid.Empty, Guid.Empty, "", "", "", "", true), new CancellationToken());

            employees.Should().NotBeNull();
            employees.TotalItems.Should().Be(4);
            employees.TotalPages.Should().Be(1);
            employees.PageNumber.Should().Be(1);
            employees.PageSize.Should().Be(10);
            employees.Items.Should().HaveCount(4);
        }
    }
}
