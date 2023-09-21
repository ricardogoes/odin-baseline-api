using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.GetDepartmentById
{
    public class GetDepartmentById : IRequestHandler<GetDepartmentByIdInput, DepartmentOutput>
    {
        private readonly IRepository<Department> _repository;
        private readonly IValidator<GetDepartmentByIdInput> _validator;

        public GetDepartmentById(IRepository<Department> repository, IValidator<GetDepartmentByIdInput> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<DepartmentOutput> Handle(GetDepartmentByIdInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }

            var department = await _repository.FindByIdAsync(input.Id, cancellationToken);
            return DepartmentOutput.FromDepartment(department);
        }
    }
}
