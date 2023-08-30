using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Departments.UpdateDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.UpdateDepartment
{
    [Collection(nameof(UpdateDepartmentTestFixtureCollection))]
    public class UpdateDepartmentInputValidatorTest
    {
        private readonly UpdateDepartmentTestFixture _fixture;

        public UpdateDepartmentInputValidatorTest(UpdateDepartmentTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Departments / UpdateDepartmentInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidUpdateDepartmentInput(Guid.Empty);
            var validator = new UpdateDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should not validate when CustomerId is empty")]
        [Trait("Application", "Departments / UpdateDepartmentInputValidator")]
        public void DontValidateWhenEmptyCustomerId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateDepartmentInputWithEmptyCustomerId();
            var validator = new UpdateDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Customer Id' must not be empty.");
        }
        [Fact(DisplayName = "Validate() should not validate when Name is empty")]
        [Trait("Application", "Departments / UpdateDepartmentInputValidator")]
        public void DontValidateWhenEmptyName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateDepartmentInputWithEmptyName();
            var validator = new UpdateDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when LoggedUsername is empty")]
        [Trait("Application", "Departments / UpdateDepartmentInputValidator")]
        public void DontValidateWhenLoggedUsernameDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdateDepartmentInputWithEmptyLoggedUsername();
            var validator = new UpdateDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Logged Username' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Departments / UpdateDepartmentInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidUpdateDepartmentInput();
            var validator = new UpdateDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
