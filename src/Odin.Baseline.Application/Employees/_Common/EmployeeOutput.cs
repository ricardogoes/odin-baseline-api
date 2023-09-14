using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Application.Employees.Common
{
    public class EmployeeOutput
    {        
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }
        public Address? Address { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public IReadOnlyList<EmployeePositionHistory>? PositionsHistory { get; private set; }
        public CustomerData? Customer { get; private set; }
        public DepartmentData? Department { get; private set; }

        public EmployeeOutput(Guid id, string firstName, string lastName, string document, string email, 
            bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy,
            Address? address = null, IReadOnlyList<EmployeePositionHistory>? positionsHistory = null, CustomerData? customer = null, DepartmentData? department = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
            Address = address;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            PositionsHistory = positionsHistory;
            Customer = customer;
            Department = department;
        }


        public static EmployeeOutput FromEmployee(Employee employee)
        {
            return new EmployeeOutput
            (
                employee.Id,                
                employee.FirstName,
                employee.LastName,
                employee.Document,
                employee.Email,                
                employee.IsActive,
                employee.CreatedAt ?? default,
                employee.CreatedBy ?? "",
                employee.LastUpdatedAt ?? default,
                employee.LastUpdatedBy ?? "",
                employee.Address, 
                employee.HistoricPositions,                
                employee.CustomerData,
                employee.DepartmentData
            );
        }

        public static IEnumerable<EmployeeOutput> FromEmployee(IEnumerable<Employee> employees)
            => employees.Select(FromEmployee);
    }
}
