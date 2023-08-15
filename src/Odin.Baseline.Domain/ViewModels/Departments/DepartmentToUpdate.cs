using System.ComponentModel.DataAnnotations;

namespace Odin.Baseline.Domain.ViewModels.Departments
{
    public class DepartmentToUpdate
    {
        [Key]
        [Required(ErrorMessage = "DepartmentId required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "CustomerId required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }
    }
}
