using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeModel>
    {
        public void Configure(EntityTypeBuilder<EmployeeModel> builder)
        {
            builder.ToTable("employees")
                .HasKey(employee => employee.Id);

            builder.Property(employee => employee.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(employee => employee.DepartmentId)
                .HasColumnName("department_id");

            builder.Property(employee => employee.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(employee => employee.LastName)
                .HasColumnName("last_name")
                .HasPrecision(2)
                .IsRequired();

            builder.Property(employee => employee.Document)
                .HasColumnName("document")
                .HasPrecision(2)
                .IsRequired();

            builder.Property(employee => employee.Email)
                .HasColumnName("email")
                .HasPrecision(2)
                .IsRequired();

            builder.Property(customer => customer.StreetName)
                .HasColumnName("street_name")
                .HasMaxLength(255);

            builder.Property(customer => customer.StreetNumber)
                .HasColumnName("street_number");

            builder.Property(customer => customer.Complement)
                .HasColumnName("complement")
                .HasMaxLength(255);

            builder.Property(customer => customer.Neighborhood)
                .HasColumnName("neighborhood")
                .HasMaxLength(255);

            builder.Property(customer => customer.ZipCode)
                .HasColumnName("zip_code")
                .HasMaxLength(255);

            builder.Property(customer => customer.City)
                .HasColumnName("city")
                .HasMaxLength(255);

            builder.Property(customer => customer.State)
                .HasColumnName("state")
                .HasMaxLength(255);

            builder.Property(employee => employee.IsActive)
               .HasColumnName("is_active")
               .IsRequired();

            builder.Property(employee => employee.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired();

            builder.Property(employee => employee.CreatedBy)
               .HasColumnName("created_by")
               .IsRequired();

            builder.Property(employee => employee.LastUpdatedAt)
               .HasColumnName("last_updated_at")
               .IsRequired();

            builder.Property(employee => employee.LastUpdatedBy)
               .HasColumnName("last_updated_by")
               .IsRequired();

            builder.Property(employee => employee.TenantId)
               .HasColumnName("tenant_id")
               .IsRequired();
        }
    }
}
