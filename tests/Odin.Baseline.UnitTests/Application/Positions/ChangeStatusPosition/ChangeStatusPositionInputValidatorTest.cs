using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Positions.ChangeStatusPosition;

namespace Odin.Baseline.UnitTests.Application.Positions.ChangeStatusPosition
{
    [Collection(nameof(ChangeStatusPositionTestFixtureCollection))]
    public class ChangeStatusPositionInputValidatorTest
    {
        private readonly ChangeStatusPositionTestFixture _fixture;

        public ChangeStatusPositionInputValidatorTest(ChangeStatusPositionTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Positions / ChangeStatusPositionInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidChangeStatusPositionInputToActivate(Guid.Empty);
            var validator = new ChangeStatusPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Action is empty")]
        [Trait("Application", "Positions / ChangeStatusPositionInputValidator")]
        public void DontValidateWhenEmptyAction()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetChangeStatusPositionInputWithEmptyAction();
            var validator = new ChangeStatusPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Action' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when LoggedUsername is empty")]
        [Trait("Application", "Positions / ChangeStatusPositionInputValidator")]
        public void DontValidateWhenLoggedUsernameDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetChangeStatusPositionInputWithEmptyLoggedUsername();
            var validator = new ChangeStatusPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Logged Username' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Positions / ChangeStatusPositionInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidChangeStatusPositionInputToActivate();
            var validator = new ChangeStatusPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
