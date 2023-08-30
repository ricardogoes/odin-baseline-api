using MediatR;
using Odin.Baseline.Application.Common;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.GetDepartments
{
    public class GetDepartments : IRequestHandler<GetDepartmentsInput, PaginatedListOutput<DepartmentOutput>>
    {
        private readonly IRepository<Department> _repository;

        public GetDepartments(IRepository<Department> repository)
            => _repository = repository;

        public async Task<PaginatedListOutput<DepartmentOutput>> Handle(GetDepartmentsInput input, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object>
            {
                { "Name", input.Name },
                { "IsActive", input.IsActive },
            };

            var departments = await _repository.FindPaginatedListAsync(
                filters, input.PageNumber, input.PageSize, input.Sort,
                cancellationToken: cancellationToken);

            return new PaginatedListOutput<DepartmentOutput>
            {
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalPages = PaginatedListHelper.GetTotalPages(departments.TotalItems, input.PageSize),
                TotalItems = departments.TotalItems,
                Items = DepartmentOutput.FromDepartment(departments.Items)
            };
        }
    }
}
