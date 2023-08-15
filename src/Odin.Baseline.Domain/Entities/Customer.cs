using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odin.Baseline.Domain.Entities
{
    [Table("customers")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Required]
        [Column("name"), StringLength(255)]
        public string Name { get; set; }

        [Column("document"), StringLength(255)]
        public string Document { get; set; }

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

        public ICollection<Department> Departments { get; set; }

        public ICollection<CompanyPosition> CompaniesPositions { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
