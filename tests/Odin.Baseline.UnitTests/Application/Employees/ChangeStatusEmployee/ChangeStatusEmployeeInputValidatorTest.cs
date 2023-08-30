using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Employees.ChangeStatusEmployee;

namespace Odin.Baseline.UnitTests.Application.Employees.ChangeStatusEmployee
{
    [Collection(nameof(ChangeStatusEmployeeTestFixtureCollection))]
    public class ChangeStatusEmployeeInputValidatorTest
    {
        private readonly ChangeStatusEmployeeTestFixture _fixture;

        public ChangeStatusEmployeeInputValidatorTest(ChangeStatusEmployeeTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Employees / ChangeStatusEmployeeInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidChangeStatusEmployeeInputToActivate(Guid.Empty);
            var validator = new ChangeStatusEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Action is empty")]
        [Trait("Application", "Employees / ChangeStatusEmployeeInputValidator")]
        public void DontValidateWhenEmptyAction()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetChangeStatusEmployeeInputWithEmptyAction();
            var validator = new ChangeStatusEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Action' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when LoggedUsername is empty")]
        [Trait("Application", "Employees / ChangeStatusEmployeeInputValidator")]
        public void DontValidateWhenLoggedUsernameDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetChangeStatusEmployeeInputWithEmptyLoggedUsername();
            var validator = new ChangeStatusEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Logged Username' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Employees / ChangeStatusEmployeeInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidChangeStatusEmployeeInputToActivate();
            var validator = new ChangeStatusEmployeeInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
