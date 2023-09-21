using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.UpdateDepartment
{
    public class UpdateDepartment : IRequestHandler<UpdateDepartmentInput, DepartmentOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Department> _repository;
        private readonly IValidator<UpdateDepartmentInput> _validator;

        public UpdateDepartment(IUnitOfWork unitOfWork, IRepository<Department> repository, IValidator<UpdateDepartmentInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<DepartmentOutput> Handle(UpdateDepartmentInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var department = await _repository.FindByIdAsync(input.Id, cancellationToken);
            department.Update(input.Name, input.CustomerId, input.LoggedUsername);

            var departmentUpdated = await _repository.UpdateAsync(department, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return DepartmentOutput.FromDepartment(departmentUpdated);
        }
    }
}
