using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Departments.GetDepartmentById;

namespace Odin.Baseline.UnitTests.Application.Departments.GetDepartmentById
{
    [Collection(nameof(GetDepartmentByIdTestFixture))]
    public class GetDepartmentByIdInputValidatorTest
    {
        private readonly GetDepartmentByIdTestFixture _fixture;

        public GetDepartmentByIdInputValidatorTest(GetDepartmentByIdTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when Id is empty")]
        [Trait("Application", "Departments / GetDepartmentByIdInputValidator")]
        public void DontValidateWhenEmptyId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidGetDepartmentByIdInput(Guid.Empty);
            var validator = new GetDepartmentByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Departments / GetDepartmentByIdInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidGetDepartmentByIdInput();
            var validator = new GetDepartmentByIdInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
