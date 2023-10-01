namespace Odin.Baseline.Domain.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }

        public Entity(Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }

        public void SetAuditLog(DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
        }
    }
}
