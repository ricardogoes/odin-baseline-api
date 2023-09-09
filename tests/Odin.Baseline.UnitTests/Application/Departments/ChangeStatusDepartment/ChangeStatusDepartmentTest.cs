﻿using FluentAssertions;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Departments.ChangeStatusDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.ChangeStatusDepartment
{
    [Collection(nameof(ChangeStatusDepartmentTestFixtureCollection))]
    public class ChangeStatusDepartmentTest
    {
        private readonly ChangeStatusDepartmentTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Department>> _repositoryMock;

        public ChangeStatusDepartmentTest(ChangeStatusDepartmentTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should activate a valid department")]
        [Trait("Application", "Departments / ChangeStatusDepartment")]
        public async Task HandleShouldActivateDepartment()
        {
            var validDepartment = _fixture.GetValidDepartment();
            var input = _fixture.GetValidChangeStatusDepartmentInputToActivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validDepartment);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validDepartment));

            var useCase = new App.ChangeStatusDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeTrue();
            
            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should deactivate a valid department")]
        [Trait("Application", "Departments / ChangeStatusDepartment")]
        public async Task HandleShouldDeactivateDepartment()
        {
            var validDepartment = _fixture.GetValidDepartment();
            var input = _fixture.GetValidChangeStatusDepartmentInputToDeactivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validDepartment);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validDepartment));

            var useCase = new App.ChangeStatusDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.IsActive.Should().BeFalse();

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when department not found")]
        [Trait("Application", "Departments / ChangeStatusDepartment")]
        public async Task ThrowWhenDepartmentNotFound()
        {
            var input = _fixture.GetValidChangeStatusDepartmentInputToActivate();

            _repositoryMock.Setup(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Department '{input.Id}' not found"));

            var useCase = new App.ChangeStatusDepartment(_unitOfWorkMock.Object, _repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(x => x.FindByIdAsync(input.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
