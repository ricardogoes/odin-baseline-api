using FluentAssertions;
using Moq;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Departments.CreateDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.CreateDepartment
{
    [Collection(nameof(CreateDepartmentTestFixtureCollection))]
    public class CreateDepartmentTest
    {
        private readonly CreateDepartmentTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Department>> _repositoryMock;

        public CreateDepartmentTest(CreateDepartmentTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should create a department with valid data")]
        [Trait("Application", "Departments / CreateDepartment")]
        public async void CreateDepartment()
        {
            var input = _fixture.GetValidCreateDepartmentInput();
            var departmentToInsert = new Department(input.CustomerId, input.Name);
            var expectedDepartmentInserted = new DepartmentOutput
            (
                Guid.NewGuid(),
                input.Name,
                true,
                DateTime.UtcNow,
                "unit.testing",
                DateTime.UtcNow,
                "unit.testing"
            );

            _repositoryMock.Setup(s => s.InsertAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(departmentToInsert));

            var useCase = new App.CreateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            _unitOfWorkMock.Verify(
                uow => uow.CommitAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(expectedDepartmentInserted.Name);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = "Handle() should throw an error when data is invalid")]
        [Trait("Application", "Departments / CreateDepartment")]
        [MemberData(
            nameof(CreateDepartmentTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(CreateDepartmentTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateDepartment(
            App.CreateDepartmentInput input,
            string exceptionMessage
        )
        {
            var useCase = new App.CreateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }
    }
}
