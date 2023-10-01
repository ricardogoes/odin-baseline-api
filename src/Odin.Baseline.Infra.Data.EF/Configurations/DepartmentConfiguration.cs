using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentModel>
    {
        public void Configure(EntityTypeBuilder<DepartmentModel> builder)
        {
            builder.ToTable("departments")
                .HasKey(department => department.Id);

            builder.Property(department => department.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(department => department.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(department => department.IsActive)
               .HasColumnName("is_active")
               .IsRequired();

            builder.Property(department => department.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired();

            builder.Property(department => department.CreatedBy)
               .HasColumnName("created_by")
               .IsRequired();

            builder.Property(department => department.LastUpdatedAt)
               .HasColumnName("last_updated_at")
               .IsRequired();

            builder.Property(department => department.LastUpdatedBy)
               .HasColumnName("last_updated_by")
               .IsRequired();

            builder.Property(department => department.TenantId)
               .HasColumnName("tenant_id")               
               .IsRequired();
        }
    }
}
