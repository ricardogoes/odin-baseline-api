﻿using FluentValidation;
using MediatR;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Enums;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.ChangeStatusDepartment
{
    public class ChangeStatusDepartment : IRequestHandler<ChangeStatusDepartmentInput, DepartmentOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Department> _repository;
        private readonly IValidator<ChangeStatusDepartmentInput> _validator;

        public ChangeStatusDepartment(IUnitOfWork unitOfWork, IRepository<Department> repository, IValidator<ChangeStatusDepartmentInput> validator)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _validator = validator;
        }

        public async Task<DepartmentOutput> Handle(ChangeStatusDepartmentInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var department = await _repository.FindByIdAsync(input.Id, cancellationToken);

            switch (input.Action)
            {
                case ChangeStatusAction.ACTIVATE:
                    department.Activate();
                    break;
                case ChangeStatusAction.DEACTIVATE:
                    department.Deactivate();
                    break;
            }

            var departmentUpdated = await _repository.UpdateAsync(department, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return DepartmentOutput.FromDepartment(departmentUpdated);
        }
    }
}
