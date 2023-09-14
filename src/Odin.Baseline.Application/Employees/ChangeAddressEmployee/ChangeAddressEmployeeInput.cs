using MediatR;
using Odin.Baseline.Application.Employees.Common;

namespace Odin.Baseline.Application.Employees.ChangeAddressEmployee
{
    public class ChangeAddressEmployeeInput : IRequest<EmployeeOutput>
    {        
        public Guid EmployeeId { get; private set; }
        public string StreetName { get; private set; }
        public int StreetNumber { get; private set; }
        public string? Complement { get; private set; }
        public string Neighborhood { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        public ChangeAddressEmployeeInput(Guid employeeId, string streetName, int streetNumber, string neighborhood, string zipCode, string city, string state, string? complement = null)
        {
            EmployeeId = employeeId;
            StreetName = streetName;
            StreetNumber = streetNumber;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public void ChangeEmployeeId(Guid employeeId)
        {
            EmployeeId = employeeId;
        }

    }
}
