using FluentAssertions;
using FluentValidation;
using Odin.Baseline.Application.Departments.GetDepartments;

namespace Odin.Baseline.UnitTests.Application.Departments.GetDepartments
{
    [Collection(nameof(GetDepartmentsTestFixtureCollection))]
    public class GetDepartmentsInputValidatorTest
    {
        private readonly GetDepartmentsTestFixture _fixture;

        public GetDepartmentsInputValidatorTest(GetDepartmentsTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Validate() should not validate when CustomerId is empty")]
        [Trait("Application", "Departments / GetDepartmentsInputValidator")]
        public void DontValidateWhenEmptyCustomerId()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = _fixture.GetValidGetDepartmentsInput(Guid.Empty);
            var validator = new GetDepartmentsInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Customer Id' must not be empty.");
        }

        [Fact(DisplayName = "Validate() should validate with valid data")]
        [Trait("Application", "Departments / GetDepartmentsInputValidator")]
        public void ValidateWhenValid()
        {
            var input = _fixture.GetValidGetDepartmentsInput(Guid.NewGuid());
            var validator = new GetDepartmentsInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
