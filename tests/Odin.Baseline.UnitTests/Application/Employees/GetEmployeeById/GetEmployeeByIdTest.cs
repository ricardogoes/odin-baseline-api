using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Employees.GetEmployeeById;

namespace Odin.Baseline.UnitTests.Application.Employees.GetEmployeeById
{
    [Collection(nameof(GetEmployeeByIdTestFixture))]
    public class GetEmployeeByIdTest
    {
        private readonly GetEmployeeByIdTestFixture _fixture;

        private readonly Mock<IEmployeeRepository> _repositoryMock;

        public GetEmployeeByIdTest(GetEmployeeByIdTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should get a employee when searched by valid Id")]
        [Trait("Application", "Employees / GetEmployeeById")]
        public async Task GetEmployeeById()
        {
            var validEmployee = _fixture.GetValidEmployee();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validEmployee);

            var input = new App.GetEmployeeByIdInput
            {
                Id = validEmployee.Id
            };

            var useCase = new App.GetEmployeeById(_repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.FirstName.Should().Be(validEmployee.FirstName);
            output.LastName.Should().Be(validEmployee.LastName);
            output.Document.Should().Be(validEmployee.Document);
            output.Email.Should().Be(validEmployee.Email);
            output.IsActive.Should().Be(validEmployee.IsActive);
            output.Id.Should().Be(validEmployee.Id);
            output.CreatedAt.Should().Be(validEmployee.CreatedAt);
        }

        [Fact(DisplayName = "Handle() should throw an error when employee does not exist")]
        [Trait("Application", "Employees / GetEmployeeById")]
        public async Task NotFoundExceptionWhenEmployeeDoesntExist()
        {
            var exampleGuid = Guid.NewGuid();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Employee '{exampleGuid}' not found"));

            var input = new App.GetEmployeeByIdInput
            {
                Id = exampleGuid
            };

            var useCase = new App.GetEmployeeById(_repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
