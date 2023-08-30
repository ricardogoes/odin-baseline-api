using MediatR;
using Odin.Baseline.Application.Employees.Common;

namespace Odin.Baseline.Application.Employees.ChangeAddressEmployee
{
    public class ChangeAddressEmployeeInput : IRequest<EmployeeOutput>
    {
        public Guid EmployeeId { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
