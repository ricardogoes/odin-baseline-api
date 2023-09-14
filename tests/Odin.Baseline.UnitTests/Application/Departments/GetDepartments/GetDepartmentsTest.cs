using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Departments.GetDepartments;

namespace Odin.Baseline.UnitTests.Application.Departments.GetDepartments
{
    [Collection(nameof(GetDepartmentsTestFixtureCollection))]
    public class GetDepartmentsTest
    {
        private readonly GetDepartmentsTestFixture _fixture;

        private readonly Mock<IRepository<Department>> _repositoryMock;

        public GetDepartmentsTest(GetDepartmentsTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should return data filtered")]
        [Trait("Application", "Departments / GetDepartments")]
        public async Task GetDepartments()
        {
            var expectedDepartments = new PaginatedListOutput<Department>
            (
                totalItems: 4,
                items: new List<Department>
                {
                    new Department(Guid.NewGuid(), _fixture.GetValidName()),
                    new Department(Guid.NewGuid(), _fixture.GetValidName()),
                    new Department(Guid.NewGuid(), _fixture.GetValidName()),
                    new Department(Guid.NewGuid(), _fixture.GetValidName())
                }
            );

            _repositoryMock.Setup(s => s.FindPaginatedListAsync(It.IsAny<Dictionary<string, object?>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedDepartments));

            var useCase = new App.GetDepartments(_repositoryMock.Object);
            var departments = await useCase.Handle(new App.GetDepartmentsInput(1, 10, Guid.NewGuid(), "name", "Unit Testing", true, "", null, null, "", null, null), new CancellationToken());

            departments.TotalItems.Should().Be(4);
        }
    }
}
