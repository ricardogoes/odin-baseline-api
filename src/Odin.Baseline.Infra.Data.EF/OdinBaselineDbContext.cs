using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Infra.Data.EF.Configurations;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF
{
    public class OdinBaselineDbContext : DbContext
    {
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<EmployeePositionHistoryModel> EmployeesPositionsHistory { get; set; }
        public DbSet<PositionModel> Positions { get; set; }

        private readonly Guid _tenantId;

        public OdinBaselineDbContext(DbContextOptions<OdinBaselineDbContext> options, ITenantService tenantService) 
            : base(options) 
        {
            _tenantId = tenantService.GetTenant();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration()).Entity<DepartmentModel>().HasQueryFilter(x => x.TenantId == _tenantId);
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration()).Entity<EmployeeModel>().HasQueryFilter(x => x.TenantId == _tenantId); ;
            modelBuilder.ApplyConfiguration(new EmployeePositionHistoryConfiguration()).Entity<EmployeePositionHistoryModel>().HasQueryFilter(x => x.TenantId == _tenantId); ;
            modelBuilder.ApplyConfiguration(new PositionConfiguration()).Entity<PositionModel>().HasQueryFilter(x => x.TenantId == _tenantId); ;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }        
    }
}
