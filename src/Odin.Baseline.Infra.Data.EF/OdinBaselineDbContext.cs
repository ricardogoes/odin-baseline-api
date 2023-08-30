using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Infra.Data.EF.Configurations;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF
{
    public class OdinBaselineDbContext : DbContext
    {
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<EmployeePositionHistoryModel> EmployeesPositionsHistory { get; set; }
        public DbSet<PositionModel> Positions { get; set; }

        public OdinBaselineDbContext(DbContextOptions<OdinBaselineDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeePositionHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
