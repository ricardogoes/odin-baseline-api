using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Configurations
{
    internal class EmployeePositionHistoryConfiguration : IEntityTypeConfiguration<EmployeePositionHistoryModel>
    {
        public void Configure(EntityTypeBuilder<EmployeePositionHistoryModel> builder)
        {
            builder.ToTable("employees_positions_history")
                .HasKey(relation => new {
                    relation.EmployeeId,
                    relation.PositionId
                });

            builder.Property(position => position.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired();

            builder.Property(position => position.PositionId)
                .HasColumnName("position_id")
                .IsRequired();

            builder.Property(position => position.Salary)
                .HasColumnName("salary")
                .HasPrecision(2)
                .IsRequired();

            builder.Property(position => position.StartDate)
               .HasColumnName("start_date")
               .IsRequired();

            builder.Property(position => position.FinishDate)
               .HasColumnName("finish_date");

            builder.Property(position => position.IsActual)
               .HasColumnName("is_actual")
               .IsRequired();
        }
    }
}
