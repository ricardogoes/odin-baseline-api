using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Departments.UpdateDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.UpdateDepartment
{
    [Collection(nameof(UpdateDepartmentTestFixtureCollection))]
    public class UpdateDepartmentTest
    {
        private readonly UpdateDepartmentTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Department>> _repositoryMock;

        public UpdateDepartmentTest(UpdateDepartmentTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Theory(DisplayName = "Handle() should update department with valid data")]
        [Trait("Application", "Departments / UpdateDepartment")]
        [MemberData(
            nameof(UpdateDepartmentTestDataGenerator.GetDepartmentsToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateDepartmentTestDataGenerator)
        )]
        public async Task UpdateDepartment(Department exampleDepartment, App.UpdateDepartmentInput input)
        {
            _repositoryMock.Setup(x => x.FindByIdAsync(exampleDepartment.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(exampleDepartment);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(exampleDepartment));

            var useCase = new App.UpdateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);

            _repositoryMock.Verify(x => x.FindByIdAsync(exampleDepartment.Id, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when department not found")]
        [Trait("Application", "Departments / UpdateDepartment")]
        public async Task ThrowWhenDepartmentNotFound()
        {
            var input = _fixture.GetValidUpdateDepartmentInput();

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Department '{input.Id}' not found"));

            var useCase = new App.UpdateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "Handle() should throw an error when cant update a department")]
        [Trait("Application", "Departments / UpdateDepartment")]
        [MemberData(
            nameof(UpdateDepartmentTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(UpdateDepartmentTestDataGenerator)
        )]
        public async Task ThrowWhenCantUpdateDepartment(App.UpdateDepartmentInput input, string expectedExceptionMessage)
        {
            var validDepartment = _fixture.GetValidDepartment();
            input.Id = validDepartment.Id;

            _repositoryMock.Setup(x => x.FindByIdAsync(validDepartment.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validDepartment);

            var useCase = new App.UpdateDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>()
                .WithMessage(expectedExceptionMessage);

            _repositoryMock.Verify(x => x.FindByIdAsync(validDepartment.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
