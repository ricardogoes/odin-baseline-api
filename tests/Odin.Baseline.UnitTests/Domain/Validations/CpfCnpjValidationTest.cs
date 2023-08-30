using FluentAssertions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Validations;

namespace Odin.Baseline.UnitTests.Domain.Validations
{
    [Collection(nameof(CpfCnpjValidationTestFixture))]
    public class CpfCnpjValidationTest
    {
        private readonly CpfCnpjValidationTestFixture _fixture;

        public CpfCnpjValidationTest(CpfCnpjValidationTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "Should validate when CNPJ is valid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void CNPJOk()
        {
            string fieldName = "Document";
            var cnpj = _fixture.GetValidCNPJ();
            Action action = () => CpfCnpjValidation.CpfCnpj(cnpj, fieldName);
            action.Should().NotThrow();
        }

        [Fact(DisplayName = "Should throw an error when CNPJ is invalid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void ThrowErrorWhenCNPJInvalid()
        {
            string? value = _fixture.GetInalidCNPJ();
            string fieldName = "Document";

            Action action =
                () => CpfCnpjValidation.CpfCnpj(value, fieldName);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be a valid CPF or CNPJ");
        }

        [Fact(DisplayName = "Should validate when CPF is valid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void CPFOk()
        {
            string fieldName = "Document";
            var cpf = _fixture.GetValidCPF();
            Action action = () => CpfCnpjValidation.CpfCnpj(cpf, fieldName);
            action.Should().NotThrow();
        }

        [Fact(DisplayName = "Should throw an error when CPF is invalid")]
        [Trait("Domain", "Validations / Cpf Cnpj Validation")]
        public void ThrowErrorWhenCPFInvalid()
        {
            string? value = _fixture.GetInvalidCPF();
            string fieldName = "Document";

            Action action =
                () => CpfCnpjValidation.CpfCnpj(value, fieldName);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be a valid CPF or CNPJ");
        }
    }
}
