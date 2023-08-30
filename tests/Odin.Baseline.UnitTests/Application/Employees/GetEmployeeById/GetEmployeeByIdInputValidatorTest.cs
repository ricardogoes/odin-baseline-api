using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Employees.GetEmployeeById;

namespace Odin.Baseline.UnitTests.Application.Employees.GetEmployeeById
{
    [Collection(nameof(GetEmployeeByIdTestFixture))]
    public class GetEmployeeByIdInputValidatorTest
    {
        private readonly GetEmployeeByIdTestFixture _fixture;

        public GetEmployeeByIdInputValidatorTest(GetEmployeeByIdTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Employees / GetEmployeeByIdInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidGetEmployeeByIdInput(Guid.Empty);
            var validator = new GetEmployeeByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Employees / GetEmployeeByIdInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidGetEmployeeByIdInput();
            var validator = new GetEmployeeByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
