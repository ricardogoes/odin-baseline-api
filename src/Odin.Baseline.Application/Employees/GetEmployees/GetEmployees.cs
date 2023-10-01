using MediatR;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;

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
                totalPages: PaginatedListOutput<EmployeeOutput>.GetTotalPages(employees.TotalItems, input.PageSize),
                totalItems: employees.TotalItems,
                items: EmployeeOutput.FromEmployee(employees.Items)
            );
        }
    }
}
