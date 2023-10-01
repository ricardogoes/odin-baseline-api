using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class PositionModelMapper
    {
        public static PositionModel ToPositionModel(this Position position, Guid tenantId)
        {
            return new PositionModel
            (
                id: position.Id,                
                name: position.Name,
                baseSalary: position.BaseSalary,
                isActive: position.IsActive,
                createdAt: position.CreatedAt ?? default,
                createdBy: position.CreatedBy ?? "",
                lastUpdatedAt: position.LastUpdatedAt ?? default,
                lastUpdatedBy: position.LastUpdatedBy ?? "",
                tenantId: tenantId
            );
        }

        public static IEnumerable<PositionModel> ToPositionModel(this IEnumerable<Position> positions, Guid tenantId)
            => positions.Select(x => ToPositionModel(x, tenantId));

        public static Position ToPosition(this PositionModel model)
        {
            var position = new Position(model.Id, model.Name, model.BaseSalary, isActive: model.IsActive);            

            position.SetAuditLog(model.CreatedAt!, model.CreatedBy!, model.LastUpdatedAt!, model.LastUpdatedBy!);

            return position;
        }

        public static IEnumerable<Position> ToPosition(this IEnumerable<PositionModel> models)
            => models.Select(ToPosition);
    }
}
