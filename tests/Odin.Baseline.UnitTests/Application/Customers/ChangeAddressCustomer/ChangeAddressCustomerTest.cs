using FluentAssertions;
using FluentValidation;
using Moq;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
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

        public ChangeAddressCustomerTest(ChangeAddressCustomerTestFixture fixture)
        {
            _fixture = fixture;
            _unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            _repositoryMock = _fixture.GetRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should throw an error CustomerId is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenCustomerIdEmpty()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithCustomerIdEmpty();
                        
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Customer Id' must not be empty.");
        }

        
        [Fact(DisplayName = "Handle() should throw an error when street name is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenStreetNameEmpty()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStreetNameEmpty();

            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Street Name' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when street number is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenStreetNumberEmpty()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStreetNumberEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Street Number' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when neighborhood is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void ThrowErrorWhenNeighborhoodEmpty()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithNeighborhoodEmpty();

            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Street Number' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when zip code is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void DontValidateWhenEmptyZipCode()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithZipCodeEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'Zip Code' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when city is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void DontValidateWhenEmptyCity()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithCityEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'City' must not be empty.");
        }

        [Fact(DisplayName = "Handle() should throw an error when state is empty")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public void DontValidateWhenEmptyState()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStateEmpty();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            action.Should().ThrowAsync<EntityValidationException>().WithMessage("'State' not be empty.");
        }

        [Fact(DisplayName = "Handle() should change address with valid data")]
        [Trait("Application", "Customers / ChangeAddressCustomer")]
        public async void ValidateWhenValid()
        {
            var validCustomer = _fixture.GetValidCustomer();
            _repositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validCustomer);

            var input = _fixture.GetValidInputAddress();
            var useCase = new App.ChangeAddressCustomer(_unitOfWorkMock.Object, _repositoryMock.Object);

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
    }
}
