using Microsoft.EntityFrameworkCore;
using Odin.Baseline.Domain.Entities;

namespace Odin.Baseline.Data.Persistence
{
    public class OdinBaselineDbContext : DbContext
    {
        public OdinBaselineDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CompanyPosition> CompaniesPositions { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}
