using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Positions.CreatePosition;

namespace Odin.Baseline.UnitTests.Application.Positions.CreatePosition
{
    [Collection(nameof(CreatePositionTestFixtureCollection))]
    public class CreateCategoryInputValidatorTest
    {
        private readonly CreatePositionTestFixture _fixture;

        public CreateCategoryInputValidatorTest(CreatePositionTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Name is empty")]
        [Trait("Application", "Positions / CreatePositionInputValidator")]
        public void DontValidateWhenEmptyName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreatePositionInputWithEmptyName();
            var validator = new CreatePositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Positions / CreatePositionInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidCreatePositionInput();
            var validator = new CreatePositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
