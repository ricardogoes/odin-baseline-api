using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class DepartmentModelMapper
    {
        public static DepartmentModel ToDepartmentModel(this Department department)
        {
            return new DepartmentModel
            (
                id: department.Id,
                customerId: department.CustomerId,
                name: department.Name,
                isActive: department.IsActive,
                createdAt: department.CreatedAt ?? default,
                createdBy: department.CreatedBy ?? "",
                lastUpdatedAt: department.LastUpdatedAt ?? default,
                lastUpdatedBy: department.LastUpdatedBy ?? ""
            );
        }

        public static IEnumerable<DepartmentModel> ToDepartmentModel(this IEnumerable<Department> departments)
            => departments.Select(ToDepartmentModel);

        public static Department ToDepartment(this DepartmentModel model)
        {
            var department = new Department(model.Id, model.CustomerId, model.Name, isActive: model.IsActive);            
            
            department.SetAuditLog(model.CreatedAt, model.CreatedBy, model.LastUpdatedAt, model.LastUpdatedBy);
            department.LoadCustomerData(new CustomerData(model.CustomerId, model.Customer!.Name));

            return department;
        }

        public static IEnumerable<Department> ToDepartment(this IEnumerable<DepartmentModel> models)
            => models.Select(ToDepartment);
    }
}
