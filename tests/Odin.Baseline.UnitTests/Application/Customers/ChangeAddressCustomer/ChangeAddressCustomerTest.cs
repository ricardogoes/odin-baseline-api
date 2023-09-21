using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Baseline.Application.Customers.ChangeAddressCustomer;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Interfaces.Repositories;
using App = Odin.Baseline.Application.Customers.ChangeAddressCustomer;

namespace Odin.Baseline.UnitTests.Application.Customers.ChangeAddressCustomer
{
    [Collection(nameof(ChangeAddressCustomerTestFixtureCollection))]
    public class ChangeAddressCustomerTest
    {
        private readonly ChangeAddressCustomerTestFixture _fixture;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly Mock<IValidator<ChangeAddressCustomerInput>> _validatorMock;

        public ChangeAddressCustomerTest(ChangeAddressCustomerTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
            _validatorMock = new();
        }

        [Fact(DisplayName = "Handle() should throw an error CustomerId is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenCustomerIdEmpty()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());


            var input = _fixture.GetInputAddressWithCustomerIdEmpty();
                        
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Customer Id' must not be empty.");
        }

        
        [Fact(DisplayName = "Handle() should throw an error when street name is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenStreetNameEmpty()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());


            var input = _fixture.GetInputAddressWithStreetNameEmpty();

            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Street Name' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when street number is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenStreetNumberEmpty()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());


            var input = _fixture.GetInputAddressWithStreetNumberEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Street Number' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when neighborhood is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenNeighborhoodEmpty()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());


            var input = _fixture.GetInputAddressWithNeighborhoodEmpty();

            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Street Number' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when zip code is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void DontValidateWhenEmptyZipCode()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetInputAddressWithZipCodeEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Zip Code' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when city is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void DontValidateWhenEmptyCity()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetInputAddressWithCityEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'City' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when state is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void DontValidateWhenEmptyState()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            var input = _fixture.GetInputAddressWithStateEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'State' not be empty.");
        }

        [Fact(DisplayName = "Handle() should change address with valid data")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public async void ValidateWhenValid()
        {
            var validCustomer = _fixture.GetValidCustomer();
            
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());

            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            var input = _fixture.GetValidInputAddress();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Address.Should().NotBeNull();
            output.Address!.StreetName.Should().Be(input.StreetName);
            output.Address.StreetNumber.Should().Be(input.StreetNumber);
            output.Address.Complement.Should().Be(input.Complement);
            output.Address.Neighborhood.Should().Be(input.Neighborhood);
            output.Address.ZipCode.Should().Be(input.ZipCode);
            output.Address.City.Should().Be(input.City);
            output.Address.State.Should().Be(input.State);

            _repositoryMock.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Handle() should throw an error when validation failed")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public async void FluentValidationFailed()
        {
            _validatorMock.Setup(s => s.ValidateAsync(It.IsAny<ChangeAddressCustomerInput>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Property", "'Property' must not be empty") })));

            var input = _fixture.GetValidInputAddress();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object, _validatorMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>();
        }
    }
}
