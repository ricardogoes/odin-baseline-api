using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Employees.CreateEmployee;

namespace Odin.Baseline.UnitTests.Application.Employees.CreateEmployee
{
    [Collection(nameof(CreateEmployeeTestFixtureCollection))]
    public class CreateEmployeeInputValidatorTest
    {
        private readonly CreateEmployeeTestFixture _fixture;

        public CreateEmployeeInputValidatorTest(CreateEmployeeTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when first name is empty")]
        [Trait("Application", "Employees / CreateEmployeeInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateEmployeeInputWithEmptyFirstName();
            var validator = new CreateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'First Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when last name is empty")]
        [Trait("Application", "Employees / CreateEmployeeInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateEmployeeInputWithEmptyLastName();
            var validator = new CreateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Last Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Document is empty")]
        [Trait("Application", "Employees / CreateEmployeeInputValidator")]
        public void DontValidateWhenEmptyDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateEmployeeInputWithEmptyDocument();
            var validator = new CreateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("Document should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Validate() should not validate when Email is empty")]
        [Trait("Application", "Employees / CreateEmployeeInputValidator")]
        public void DontValidateWhenEmailEmpty()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateEmployeeInputWithInvalidEmail();
            var validator = new CreateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email' is not a valid email address.");
        }

        [Fact(DisplayName = "Validate() should not validate when LoggedUsername is empty")]
        [Trait("Application", "Employees / CreateEmployeeInputValidator")]
        public void DontValidateWhenLoggedUsernameDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateEmployeeInputWithEmptyLoggedUsername();
            var validator = new CreateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Logged Username' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Employees / CreateEmployeeInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidCreateEmployeeInput();
            var validator = new CreateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
