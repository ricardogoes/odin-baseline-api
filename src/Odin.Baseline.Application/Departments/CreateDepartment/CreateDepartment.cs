using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.CreateDepartment
{
    public class CreateDepartment : IRequestHandler<CreateDepartmentInput, DepartmentOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Department> _repository;
        private readonly IValidator<CreateDepartmentInput> _validator;

        public CreateDepartment(IUnitOfWork unitOfWork, IRepository<Department> repository, IValidator<CreateDepartmentInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<DepartmentOutput> Handle(CreateDepartmentInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var position = new Department(input.CustomerId, input.Name);
            position.Create(input.LoggedUsername);

            await _repository.InsertAsync(position, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return DepartmentOutput.FromDepartment(position);
        }
    }
}
