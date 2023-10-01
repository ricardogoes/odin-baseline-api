using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Departments.CreateDepartment;

namespace Odin.Baseline.UnitTests.Application.Departments.CreateDepartment
{
    [Collection(nameof(CreateDepartmentTestFixtureCollection))]
    public class CreateCategoryInputValidatorTest
    {
        private readonly CreateDepartmentTestFixture _fixture;

        public CreateCategoryInputValidatorTest(CreateDepartmentTestFixture fixture)
            => _fixture = fixture;
                
        [Fact(DisplayName = "Validate() should not validate when Name is empty")]
        [Trait("Application", "Departments / CreateDepartmentInputValidator")]
        public void DontValidateWhenEmptyName()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetCreateDepartmentInputWithEmptyName();
            var validator = new CreateDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Name' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Departments / CreateDepartmentInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidCreateDepartmentInput();
            var validator = new CreateDepartmentInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
