using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Employees.ChangeAddressEmployee;

namespace Odin.Baseline.UnitTests.Application.Employees.ChangeAddressEmployee
{
    [Collection(nameof(ChangeAddressEmployeeTestFixtureCollection))]
    public class ChangeAddressEmployeeInputValidatorTest
    {
        private readonly ChangeAddressEmployeeTestFixture _fixture;

        public ChangeAddressEmployeeInputValidatorTest(ChangeAddressEmployeeTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when EmployeeId is empty")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithEmployeeIdEmpty();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Employee Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when street name is empty")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void DontValidateWhenEmptyStreetName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStreetNameEmpty();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Street Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when street number is empty")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void DontValidateWhenEmptyStreetNumber()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStreetNumberEmpty();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Street Number' must be greater than '0'.");
        }

        [Fact(DisplayName = "Validate() should not validate when neighborhood is empty")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void DontValidateWhenEmptyNeighborhood()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithNeighborhoodEmpty();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Neighborhood' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when zip code is empty")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void DontValidateWhenEmptyZipCode()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithZipCodeEmpty();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Zip Code' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when city is empty")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void DontValidateWhenEmptyCity()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithCityEmpty();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'City' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when state is empty")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void DontValidateWhenEmptyState()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputAddressWithStateEmpty();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'State' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Employees / ChangeAddressEmployeeInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidInputAddress();
            var validator = new ChangeAddressEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
