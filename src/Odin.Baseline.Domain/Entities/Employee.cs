using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odin.Baseline.Domain.Entities
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        [Column("customer_id")]
        public int CustomerId { get; set; }
                
        [ForeignKey(nameof(Department))]
        [Column("department_id")]
        public int? DepartmentId { get; set; }

        [Required]
        [ForeignKey(nameof(CompanyPosition))]
        [Column("position_id")]
        public int CompanyPositionId { get; set; }

        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        [EmailAddress]
        [Column("email"), StringLength(255)]        
        public string Email { get; set; }

        [Column("salary")]
        public decimal? Salary { get; set; }

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

        public Customer Customer { get; set; } = null;
        
        public Department Department { get; set; } = null;
        
        public CompanyPosition CompanyPosition { get; set; } = null;
    }
}
