using Odin.Baseline.Domain.DTO;
using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Application.Positions
{
    public class PositionOutput
    {

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal BaseSalary { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public CustomerData? Customer { get; private set; }

        public PositionOutput(Guid id, string name, decimal baseSalary, bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, CustomerData? customer = null)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            Customer = customer;
        }


        public static PositionOutput FromPosition(Position position)
        {
            return new PositionOutput
            (
                position.Id,
                position.Name,
                position.BaseSalary ?? 0,
                position.IsActive,
                position.CreatedAt ?? default,
                position.CreatedBy ?? "",
                position.LastUpdatedAt ?? default,
                position.LastUpdatedBy ?? ""
            );
        }

        public static IEnumerable<PositionOutput> FromPosition(IEnumerable<Position> positions)
            => positions.Select(position => FromPosition(position));
    }
}
