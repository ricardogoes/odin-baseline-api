namespace Odin.Baseline.Infra.Data.EF.Models
{
    public class PositionModel : BaseModel
    {       
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }
        public bool IsActive { get; private set; }
        
        public PositionModel(Guid id, string name, decimal? baseSalary, bool isActive, DateTime createdAt, string createdBy, DateTime lastUpdatedAt, string lastUpdatedBy, Guid tenantId)
            : base(id, createdAt, createdBy, lastUpdatedAt, lastUpdatedBy, tenantId)
        {
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;            
        }
    }
}
