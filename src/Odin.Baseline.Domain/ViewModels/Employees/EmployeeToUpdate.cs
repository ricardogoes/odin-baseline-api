using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Baseline.Domain.ViewModels.Employees
{
    public class EmployeeToUpdate
    {
        [Key]
        [Required(ErrorMessage = "EmployeeId required")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "CustomerId required")]
        public int CustomerId { get; set; }
        
        public int? DepartmentId { get; set; }

        public int? CompanyPositionId { get; set; }

        [Required(ErrorMessage = "FirstName required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string Email { get; set; }

        public decimal? Salary { get; set; }        
    }
}
