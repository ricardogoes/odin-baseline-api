using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ValueObjects;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class EmployeeModelMapper
    {
        public static EmployeeModel ToEmployeeModel(this Employee employee)
        {
            return new EmployeeModel
            {
                Id = employee.Id,
                CustomerId = employee.CustomerId,
                DepartmentId = employee.DepartmentId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Document = employee.Document,
                Email = employee.Email,
                StreetName = employee.Address?.StreetName,
                StreetNumber = employee.Address?.StreetNumber,
                Complement = employee.Address?.Complement,
                Neighborhood = employee.Address?.Neighborhood,
                ZipCode = employee.Address?.ZipCode,
                City = employee.Address?.City,
                State = employee.Address?.State,
                IsActive = employee.IsActive,
                CreatedAt = employee.CreatedAt,
                CreatedBy = employee.CreatedBy,
                LastUpdatedAt = employee.LastUpdatedAt,
                LastUpdatedBy = employee.LastUpdatedBy
            };
        }

        public static IEnumerable<EmployeeModel> ToEmployeeModel(this IEnumerable<Employee> employees)
            => employees.Select(ToEmployeeModel);

        public static Employee ToEmployee(this EmployeeModel model)
        {
            var employee = new Employee(model.Id, model.CustomerId, model.FirstName, model.LastName, model.Document, model.Email, 
                departmentId: model.DepartmentId, 
                isActive: model.IsActive);

            if (!string.IsNullOrWhiteSpace(model.StreetName))
            {
                var address = new Address(model.StreetName, model.StreetNumber ?? 0, model.Complement, model.Neighborhood, model.ZipCode, model.City, model.State);
                employee.ChangeAddress(address);
            }

            employee.SetAuditLog(model.CreatedAt, model.CreatedBy, model.LastUpdatedAt, model.LastUpdatedBy);

            return employee;
        }

        public static IEnumerable<Employee> ToEmployee(this IEnumerable<EmployeeModel> models)
            => models.Select(ToEmployee);
    }
}
