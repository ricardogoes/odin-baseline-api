using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Positions.GetPositions;

namespace Odin.Baseline.UnitTests.Application.Positions.GetPositions
{
    [Collection(nameof(GetPositionsTestFixtureCollection))]
    public class GetPositionsInputValidatorTest
    {
        private readonly GetPositionsTestFixture _fixture;

        public GetPositionsInputValidatorTest(GetPositionsTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when CustomerId is empty")]
        [Trait("Application", "Positions / GetPositionsInputValidator")]
        public void DontValidateWhenEmptyCustomerId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidGetPositionsInput(Guid.Empty);
            var validator = new GetPositionsInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Customer Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Positions / GetPositionsInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidGetPositionsInput(Guid.NewGuid());
            var validator = new GetPositionsInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
