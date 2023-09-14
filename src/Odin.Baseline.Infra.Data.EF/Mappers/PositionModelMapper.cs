using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class PositionModelMapper
    {
        public static PositionModel ToPositionModel(this Position position)
        {
            return new PositionModel
            (
                id: position.Id,
                customerId: position.CustomerId,
                name: position.Name,
                baseSalary: position.BaseSalary,
                isActive: position.IsActive,
                createdAt: position.CreatedAt ?? default,
                createdBy: position.CreatedBy ?? "",
                lastUpdatedAt: position.LastUpdatedAt ?? default,
                lastUpdatedBy: position.LastUpdatedBy ?? ""
            );
        }

        public static IEnumerable<PositionModel> ToPositionModel(this IEnumerable<Position> positions)
            => positions.Select(ToPositionModel);

        public static Position ToPosition(this PositionModel model)
        {
            var position = new Position(model.Id, model.CustomerId, model.Name, model.BaseSalary, isActive: model.IsActive);            

            position.SetAuditLog(model.CreatedAt, model.CreatedBy, model.LastUpdatedAt, model.LastUpdatedBy);
            position.LoadCustomerData(new CustomerData(model.CustomerId, model.Customer!.Name));

            return position;
        }

        public static IEnumerable<Position> ToPosition(this IEnumerable<PositionModel> models)
            => models.Select(ToPosition);
    }
}
