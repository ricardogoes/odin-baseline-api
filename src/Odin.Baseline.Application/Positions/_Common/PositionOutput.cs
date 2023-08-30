using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Application.Positions.Common
{
    public class PositionOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal BaseSalary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public static PositionOutput FromPosition(Position position)
        {
            return new PositionOutput
            {
                Id = position.Id,
                Name = position.Name,
                BaseSalary = position.BaseSalary ?? 0,
                IsActive = position.IsActive,
                CreatedAt = position.CreatedAt,
                CreatedBy = position.CreatedBy,
                LastUpdatedAt = position.LastUpdatedAt,
                LastUpdatedBy = position.LastUpdatedBy

            };
        }

        public static IEnumerable<PositionOutput> FromPosition(IEnumerable<Position> positions)
            => positions.Select(position => FromPosition(position));
    }
}
