using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Configurations
{
    internal class PositionConfiguration : IEntityTypeConfiguration<PositionModel>
    {
        public void Configure(EntityTypeBuilder<PositionModel> builder)
        {
            builder.ToTable("positions")
                .HasKey(position => position.Id);

            builder.Property(position => position.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(position => position.CustomerId)
                .HasColumnName("customer_id")
                .IsRequired();

            builder.Property(position => position.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(position => position.BaseSalary)
                .HasColumnName("base_salary")
                .HasPrecision(2);

            builder.Property(position => position.IsActive)
               .HasColumnName("is_active")
               .IsRequired();

            builder.Property(position => position.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired();

            builder.Property(position => position.CreatedBy)
               .HasColumnName("created_by")
               .IsRequired();

            builder.Property(position => position.LastUpdatedAt)
               .HasColumnName("last_updated_at")
               .IsRequired();

            builder.Property(position => position.LastUpdatedBy)
               .HasColumnName("last_updated_by")
               .IsRequired();
        }
    }
}
