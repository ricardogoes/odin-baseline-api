using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.GetDepartmentById
{
    public class GetDepartmentById : IRequestHandler<GetDepartmentByIdInput, DepartmentOutput>
    {
        private readonly IRepository<Department> _repository;

        public GetDepartmentById(IRepository<Department> repository) 
            => _repository = repository;

        public async Task<DepartmentOutput> Handle(GetDepartmentByIdInput input, CancellationToken cancellationToken)
        {
            var department = await _repository.FindByIdAsync(input.Id, cancellationToken);
            return DepartmentOutput.FromDepartment(department);
        }
    }
}
