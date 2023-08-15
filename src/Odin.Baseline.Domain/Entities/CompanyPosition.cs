using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odin.Baseline.Domain.Entities
{
    [Table("companies_positions")]
    public class CompanyPosition
    {
        [Key]
        [Column("position_id")]
        public int PositionId { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        [Column("customer_id")] 
        public int CustomerId { get; set; }

        [Required]
        [Column("name"), StringLength(255)]
        public string Name { get; set; }

        [Column("base_salary")] 
        public decimal? BaseSalary { get; set; }

        [Required]
        [Column("is_active")] 
        public bool IsActive { get; set; }

        [Required]
        [Column("created_by"), StringLength(255)]
        public string CreatedBy { get; set; }

        [Required]
        [Column("created_at")] 
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("last_updated_by"), StringLength(255)]
        public string LastUpdatedBy { get; set; }

        [Required]
        [Column("last_updated_at")] 
        public DateTime LastUpdatedAt { get; set; }

        public Customer Customer { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
