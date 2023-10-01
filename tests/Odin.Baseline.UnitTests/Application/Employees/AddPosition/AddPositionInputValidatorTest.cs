using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Employees.AddPosition;

namespace Odin.Baseline.UnitTests.Application.Employees.AddPosition
{
    [Collection(nameof(AddPositionTestFixtureCollection))]
    public class AddPositionInputValidatorTest
    {
        private readonly AddPositionTestFixture _fixture;

        public AddPositionInputValidatorTest(AddPositionTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when EmployeeId is empty")]
        [Trait("Application", "Employees / AddPositionInputValidator")]
        public void DontValidateWhenEmptyEmployeeId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptyEmployeeId();
            var validator = new AddPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Employee Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when PositionId is empty")]
        [Trait("Application", "Employees / AddPositionInputValidator")]
        public void DontValidateWhenEmptyPositionId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptyPositionId();
            var validator = new AddPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Position Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should not validate when Salary is empty")]
        [Trait("Application", "Employees / AddPositionInputValidator")]
        public void DontValidateWhenEmptySalary()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetInputWithEmptySalary();
            var validator = new AddPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Salary' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Employees / AddPositionInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidInput();
            var validator = new AddPositionInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
