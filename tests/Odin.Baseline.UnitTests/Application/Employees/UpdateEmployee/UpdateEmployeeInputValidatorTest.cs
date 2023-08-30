using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Employees.UpdateEmployee;

namespace Odin.Baseline.UnitTests.Application.Employees.UpdateEmployee
{
    [Collection(nameof(UpdateEmployeeTestFixtureCollection))]
    public class UpdateEmployeeInputValidatorTest
    {
        private readonly UpdateEmployeeTestFixture _fixture;

        public UpdateEmployeeInputValidatorTest(UpdateEmployeeTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Employees / UpdateEmployeeInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidUpdateEmployeeInput(Guid.Empty);
            var validator = new UpdateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when First Name is empty")]
        [Trait("Application", "Employees / UpdateEmployeeInputValidator")]
        public void DontValidateWhenEmptyFirstName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateEmployeeInputWithEmptyFirstName();
            var validator = new UpdateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'First Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Last Name is empty")]
        [Trait("Application", "Employees / UpdateEmployeeInputValidator")]
        public void DontValidateWhenEmptyLastName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateEmployeeInputWithEmptyLastName();
            var validator = new UpdateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Last Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Document is empty")]
        [Trait("Application", "Employees / UpdateEmployeeInputValidator")]
        public void DontValidateWhenEmptyDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateEmployeeInputWithEmptyDocument();
            var validator = new UpdateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("Document should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Validate() should not validate when Email is empty")]
        [Trait("Application", "Employees / UpdateEmployeeInputValidator")]
        public void DontValidateWhenEmailEmpty()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateEmployeeInputWithInvalidEmail();
            var validator = new UpdateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Email' is not a valid email address.");
        }

        [Fact(DisplayName = "Validate() should not validate when LoggedUsername is empty")]
        [Trait("Application", "Employees / UpdateEmployeeInputValidator")]
        public void DontValidateWhenLoggedUsernameDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateEmployeeInputWithEmptyLoggedUsername();
            var validator = new UpdateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Logged Username' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Employees / UpdateEmployeeInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidUpdateEmployeeInput();
            var validator = new UpdateEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
