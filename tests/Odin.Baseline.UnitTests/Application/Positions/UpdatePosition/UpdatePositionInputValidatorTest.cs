using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Positions.UpdatePosition;

namespace Odin.Baseline.UnitTests.Application.Positions.UpdatePosition
{
    [Collection(nameof(UpdatePositionTestFixtureCollection))]
    public class UpdatePositionInputValidatorTest
    {
        private readonly UpdatePositionTestFixture _fixture;

        public UpdatePositionInputValidatorTest(UpdatePositionTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Positions / UpdatePositionInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidUpdatePositionInput(Guid.Empty);
            var validator = new UpdatePositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should not validate when CustomerId is empty")]
        [Trait("Application", "Positions / UpdatePositionInputValidator")]
        public void DontValidateWhenEmptyCustomerId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdatePositionInputWithEmptyCustomerId();
            var validator = new UpdatePositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Customer Id' must not be empty.");
        }
        [Fact(DisplayName = "Validate() should not validate when Name is empty")]
        [Trait("Application", "Positions / UpdatePositionInputValidator")]
        public void DontValidateWhenEmptyName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdatePositionInputWithEmptyName();
            var validator = new UpdatePositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when LoggedUsername is empty")]
        [Trait("Application", "Positions / UpdatePositionInputValidator")]
        public void DontValidateWhenLoggedUsernameDocument()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetUpdatePositionInputWithEmptyLoggedUsername();
            var validator = new UpdatePositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Logged Username' must not be empty.");
        }


        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Positions / UpdatePositionInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidUpdatePositionInput();
            var validator = new UpdatePositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
