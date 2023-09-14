using MediatR;
using Odin.Baseline.Application.Common;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Employees.GetEmployees
{
    public class GetEmployees : IRequestHandler<GetEmployeesInput, PaginatedListOutput<EmployeeOutput>>
    {
        private readonly IEmployeeRepository _repository;

        public GetEmployees(IEmployeeRepository repository)
            => _repository = repository;

        public async Task<PaginatedListOutput<EmployeeOutput>> Handle(GetEmployeesInput input, CancellationToken cancellationToken)
        {
            var filters = new Dictionary<string, object?>
            {
                { "CustomerId", input.CustomerId },
                { "DepartmentId", input.DepartmentId },
                { "FirstName", input.FirstName },
                { "LastName", input.LastName },
                { "Document", input.Document },
                { "Email", input.Email },
                { "IsActive", input.IsActive },
            };

            var employees = await _repository.FindPaginatedListAsync(
                filters, input.PageNumber, input.PageSize, input.Sort!,
                cancellationToken: cancellationToken);

            return new PaginatedListOutput<EmployeeOutput>
            (
                pageNumber: input.PageNumber,
                pageSize: input.PageSize,
                totalPages: PaginatedListHelper.GetTotalPages(employees.TotalItems, input.PageSize),
                totalItems: employees.TotalItems,
                items: EmployeeOutput.FromEmployee(employees.Items)
            );
        }
    }
}
