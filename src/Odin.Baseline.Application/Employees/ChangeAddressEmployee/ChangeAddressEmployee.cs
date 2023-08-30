using MediatR;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Application.Employees.ChangeAddressEmployee
{
    public class ChangeAddressEmployee : IRequestHandler<ChangeAddressEmployeeInput, EmployeeOutput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _repository;

        public ChangeAddressEmployee(IUnitOfWork unitOfWork, IEmployeeRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<EmployeeOutput> Handle(ChangeAddressEmployeeInput request, CancellationToken cancellationToken)
        {
            var employee = await _repository.FindByIdAsync(request.EmployeeId, cancellationToken);
            var address = new Address(request.StreetName, request.StreetNumber, request.Complement, request.Neighborhood, request.ZipCode, request.City, request.State);

            employee.ChangeAddress(address);

            await _repository.UpdateAsync(employee);
            await _unitOfWork.CommitAsync(cancellationToken);

            return EmployeeOutput.FromEmployee(employee);
        }
    }
}
