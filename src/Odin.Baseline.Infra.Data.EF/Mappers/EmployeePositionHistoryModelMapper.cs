using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class EmployeePositionHistoryModelMapper
    {
        public static EmployeePositionHistoryModel ToEmployeePositionHistoryModel(this EmployeePositionHistory positionHistory, Guid employeeId)
        {
            return new EmployeePositionHistoryModel
            (
                employeeId,
                positionHistory.PositionId,
                positionHistory.Salary,
                positionHistory.StartDate,
                positionHistory.FinishDate,                
                positionHistory.IsActual
            );
        }

        public static IEnumerable<EmployeePositionHistoryModel> ToEmployeePositionHistoryModel(this IEnumerable<EmployeePositionHistory> positionHistorys, Guid employeeId)
            => positionHistorys.Select(positionHistory => ToEmployeePositionHistoryModel(positionHistory, employeeId));

        public static EmployeePositionHistory ToEmployeePositionHistory(this EmployeePositionHistoryModel model)
        {
            var positionHistory = new EmployeePositionHistory(model.PositionId, model.Salary, model.StartDate, model.FinishDate, model.IsActual);            
            return positionHistory;
        }

        public static List<EmployeePositionHistory> ToEmployeePositionHistory(this IEnumerable<EmployeePositionHistoryModel> models)
            => models.Select(ToEmployeePositionHistory).ToList();
    }
}
