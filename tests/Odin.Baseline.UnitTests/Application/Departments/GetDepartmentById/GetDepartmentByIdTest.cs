using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Departments.GetDepartmentById;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Departments.GetDepartmentById;

namespace Odin.Baseline.UnitTests.Application.Departments.GetDepartmentById
{
    [Collection(nameof(GetDepartmentByIdTestFixture))]
    public class GetDepartmentByIdTest
    {
        private readonly GetDepartmentByIdTestFixture _fixture;

        private readonly Mock<IRepository<Department>> _repositoryMock;
        private readonly Mock<IValidator<GetDepartmentByIdInput>> _validatorMock;

        public GetDepartmentByIdTest(GetDepartmentByIdTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should get a department when searched by valid Id")]
        [Trait("Application", "Departments / GetDepartmentById")]
        public async Task GetDepartmentById()
        {
            var validDepartment = _fixture.GetValidDepartment();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetDepartmentByIdInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validDepartment);

            var input = new App.GetDepartmentByIdInput
            {
                Id = validDepartment.Id
            };

            var useCase = new App.GetDepartmentById(_repositoryMock.Object, _validatorMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(validDepartment.Name);
            output.IsActive.Should().Be(validDepartment.IsActive);
            output.Id.Should().Be(validDepartment.Id);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Departments / GetDepartmentById")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetDepartmentByIdInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var useCase = new App.GetDepartmentById(_repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(new App.GetDepartmentByIdInput { Id = Guid.NewGuid() }, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }

        [Fact(DisplayName = "Handle() should throw an error when department does not exist")]
        [Trait("Application", "Departments / GetDepartmentById")]
        public async Task NotFoundExceptionWhenDepartmentDoesntExist()
        {
            var exampleGuid = Guid.NewGuid();

            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<GetDepartmentByIdInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Department '{exampleGuid}' not found"));

            var input = new App.GetDepartmentByIdInput
            {
                Id = exampleGuid
            };

            var useCase = new App.GetDepartmentById(_repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
