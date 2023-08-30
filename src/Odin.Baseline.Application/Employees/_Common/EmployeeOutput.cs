using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Application.Employees.Common
{
    public class EmployeeOutput
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public IReadOnlyList<EmployeePositionHistory> PositionsHistory { get; set; }

        public static EmployeeOutput FromEmployee(Employee employee)
        {
            return new EmployeeOutput
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Document = employee.Document,
                Email = employee.Email,
                Address = employee.Address,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                CreatedBy = employee.CreatedBy,
                LastUpdatedAt = employee.LastUpdatedAt,
                LastUpdatedBy = employee.LastUpdatedBy,
                PositionsHistory = employee.HistoricPositions
            };
        }

        public static IEnumerable<EmployeeOutput> FromEmployee(IEnumerable<Employee> employees)
            => employees.Select(FromEmployee);
    }
}
