using FluentValidation;
using MediatR;
using Odin.Baseline.Application.Common;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.GetDepartments
{
    public class GetDepartments : IRequestHandler<GetDepartmentsInput, PaginatedListOutput<DepartmentOutput>>
    {
        private readonly IRepository<Department> _repository;
        private readonly IValidator<GetDepartmentsInput> _validator;

        public GetDepartments(IRepository<Department> repository, IValidator<GetDepartmentsInput> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<PaginatedListOutput<DepartmentOutput>> Handle(GetDepartmentsInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException($"One or more validation errors occurred on type {nameof(input)}.", validationResult.ToDictionary());
            }
            
            var filters = new Dictionary<string, object?>
            {
                { "CustomerId", input.CustomerId },
                { "Name", input.Name },
                { "IsActive", input.IsActive },
                { "CreatedBy", input.CreatedBy },
                { "CreatedAtStart", input.CreatedAtStart },
                { "CreatedAtEnd", input.CreatedAtEnd },
                { "LastUpdatedBy", input.LastUpdatedBy },
                { "LastUpdatedAtStart", input.LastUpdatedAtStart },
                { "LastUpdatedAtEnd", input.LastUpdatedAtEnd },
            };

            var departments = await _repository.FindPaginatedListAsync(
                filters, input.PageNumber, input.PageSize, input.Sort!,
                cancellationToken: cancellationToken);

            return new PaginatedListOutput<DepartmentOutput>
            (
                pageNumber: input.PageNumber,
                pageSize: input.PageSize,
                totalPages: PaginatedListHelper.GetTotalPages(departments.TotalItems, input.PageSize),
                totalItems: departments.TotalItems,
                items: DepartmentOutput.FromDepartment(departments.Items)
            );
        }
    }
}
