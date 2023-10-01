using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ValueObjects;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class EmployeeModelMapper
    {
        public static EmployeeModel ToEmployeeModel(this Employee employee, Guid tenantId)
        {
            return new EmployeeModel
            (
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.Document,
                employee.Email,
                employee.Address?.StreetName,
                employee.Address?.StreetNumber,
                employee.Address?.Complement,
                employee.Address?.Neighborhood,
                employee.Address?.ZipCode,
                employee.Address?.City,
                employee.Address?.State,
                employee.IsActive,
                employee.CreatedAt ?? default,
                employee.CreatedBy ?? "",
                employee.LastUpdatedAt ?? default,
                employee.LastUpdatedBy ?? "",
                tenantId,
                employee.DepartmentId
            );
        }

        public static IEnumerable<EmployeeModel> ToEmployeeModel(this IEnumerable<Employee> employees, Guid tenantId)
            => employees.Select(x => ToEmployeeModel(x, tenantId));

        public static Employee ToEmployee(this EmployeeModel model)
        {
            var employee = new Employee(model.Id, model.FirstName, model.LastName, model.Document, model.Email, 
                departmentId: model.DepartmentId, 
                isActive: model.IsActive);

            if (!string.IsNullOrWhiteSpace(model.StreetName))
            {
                var address = new Address(model.StreetName, model.StreetNumber ?? 0, model.Complement ?? "", model.Neighborhood!, model.ZipCode!, model.City!, model.State!);
                employee.ChangeAddress(address);
            }

            if(model.DepartmentId.HasValue && model.DepartmentId.Value != Guid.Empty)
            {
                employee.LoadDepartmentData(new DepartmentData(model.DepartmentId.Value, model.Department!.Name));
            }

            if(model.HistoricPositions is not null && model.HistoricPositions.Any())
            {
                foreach(var historicPositionModel in model.HistoricPositions)
                {
                    employee.LoadHistoricPosition(historicPositionModel.ToEmployeePositionHistory());
                }
            }

            employee.SetAuditLog(model.CreatedAt!, model.CreatedBy!, model.LastUpdatedAt!, model.LastUpdatedBy!);

            return employee;
        }

        public static IEnumerable<Employee> ToEmployee(this IEnumerable<EmployeeModel> models)
            => models.Select(ToEmployee);
    }
}
