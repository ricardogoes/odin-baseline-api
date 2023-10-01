using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class DepartmentModelMapper
    {
        public static DepartmentModel ToDepartmentModel(this Department department, Guid tenantId)
        {
            var model = new DepartmentModel
            (
                id: department.Id,
                name: department.Name,
                isActive: department.IsActive,
                createdAt: department.CreatedAt ?? default,
                createdBy: department.CreatedBy ?? "",
                lastUpdatedAt: department.LastUpdatedAt ?? default,
                lastUpdatedBy: department.LastUpdatedBy ?? "",
                tenantId: tenantId
            );

            return model;
        }

        public static IEnumerable<DepartmentModel> ToDepartmentModel(this IEnumerable<Department> departments, Guid tenantId)
            => departments.Select(x => ToDepartmentModel(x, tenantId));

        public static Department ToDepartment(this DepartmentModel model)
        {
            var department = new Department(model.Id, model.Name, isActive: model.IsActive);            
            
            department.SetAuditLog(model.CreatedAt!, model.CreatedBy!, model.LastUpdatedAt!, model.LastUpdatedBy!);

            return department;
        }

        public static IEnumerable<Department> ToDepartment(this IEnumerable<DepartmentModel> models)
            => models.Select(ToDepartment);
    }
}
