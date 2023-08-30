using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Positions.GetPositionById;

namespace Odin.Baseline.UnitTests.Application.Positions.GetPositionById
{
    [Collection(nameof(GetPositionByIdTestFixture))]
    public class GetPositionByIdInputValidatorTest
    {
        private readonly GetPositionByIdTestFixture _fixture;

        public GetPositionByIdInputValidatorTest(GetPositionByIdTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Positions / GetPositionByIdInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidGetPositionByIdInput(Guid.Empty);
            var validator = new GetPositionByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Positions / GetPositionByIdInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidGetPositionByIdInput();
            var validator = new GetPositionByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
