using MediatR;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;

namespace Odin.Baseline.Application.Departments.CreateDepartment
{
    public class CreateDepartment : IRequestHandler<CreateDepartmentInput, DepartmentOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Department> _repository;

        public CreateDepartment(IUnitOfWork unitOfWork, IRepository<Department> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<DepartmentOutput> Handle(CreateDepartmentInput input, CancellationToken cancellationToken)
        {
            var position = new Department(input.CustomerId, input.Name);
            position.Create(input.LoggedUsername);

            await _repository.InsertAsync(position, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return DepartmentOutput.FromDepartment(position);
        }
    }
}
