using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Departments.ChangeStatusDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.ChangeStatusDepartment
{
    [Collection(nameof(ChangeStatusDepartmentTestFixtureCollection))]
    public class ChangeStatusDepartmentInputValidatorTest
    {
        private readonly ChangeStatusDepartmentTestFixture _fixture;

        public ChangeStatusDepartmentInputValidatorTest(ChangeStatusDepartmentTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Departments / ChangeStatusDepartmentInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidChangeStatusDepartmentInputToActivate(Guid.Empty);
            var validator = new ChangeStatusDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Action is empty")]
        [Trait("Application", "Departments / ChangeStatusDepartmentInputValidator")]
        public void DontValidateWhenEmptyAction()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetChangeStatusDepartmentInputWithEmptyAction();
            var validator = new ChangeStatusDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Action' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when LoggedUsername is empty")]
        [Trait("Application", "Departments / ChangeStatusDepartmentInputValidator")]
        public void DontValidateWhenEmptyLoggedUsername()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetChangeStatusDepartmentInputWithEmptyLoggedUsername();
            var validator = new ChangeStatusDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Logged Username' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Departments / ChangeStatusDepartmentInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidChangeStatusDepartmentInputToActivate();
            var validator = new ChangeStatusDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
