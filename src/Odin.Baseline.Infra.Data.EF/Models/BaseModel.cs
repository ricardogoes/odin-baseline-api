namespace Odin.Baseline.Infra.Data.EF.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public string CreatedBy { get; protected set; }
        public DateTime LastUpdatedAt { get; protected set; }
        public string LastUpdatedBy { get; protected set; }
        public Guid TenantId { get; protected set; }

        protected BaseModel(Guid id, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid tenantId)
        {
            Id = id;
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedAt;
            LastUpdatedBy = lastUpdatedBy;
            TenantId = tenantId;
        }

        public void SetAuditLog(string currentUsername, bool created)
        {
            if(created)
            {
                CreatedAt = DateTime.UtcNow;
                CreatedBy = currentUsername;
            }

            LastUpdatedAt = DateTime.UtcNow;
            LastUpdatedBy = currentUsername;
        }
    }
}
