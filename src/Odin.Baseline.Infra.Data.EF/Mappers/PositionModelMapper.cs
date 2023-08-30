using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class PositionModelMapper
    {
        public static PositionModel ToPositionModel(this Position position)
        {
            return new PositionModel
            {
                Id = position.Id,
                CustomerId = position.CustomerId,
                Name = position.Name,
                BaseSalary = position.BaseSalary,
                IsActive = position.IsActive,
                CreatedAt = position.CreatedAt,
                CreatedBy = position.CreatedBy,
                LastUpdatedAt = position.LastUpdatedAt,
                LastUpdatedBy = position.LastUpdatedBy
            };
        }

        public static IEnumerable<PositionModel> ToPositionModel(this IEnumerable<Position> positions)
            => positions.Select(ToPositionModel);

        public static Position ToPosition(this PositionModel model)
        {
            var position = new Position(model.Id, model.CustomerId, model.Name, model.BaseSalary, isActive: model.IsActive);            
            position.SetAuditLog(model.CreatedAt, model.CreatedBy, model.LastUpdatedAt, model.LastUpdatedBy);

            return position;
        }

        public static IEnumerable<Position> ToPosition(this IEnumerable<PositionModel> models)
            => models.Select(ToPosition);
    }
}
