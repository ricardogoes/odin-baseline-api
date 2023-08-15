using System.ComponentModel.DataAnnotations;

namespace Odin.Baseline.Domain.ViewModels.Departments
{
    public class DepartmentToInsert
    {
        [Required(ErrorMessage = "CustomerId required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }
    }
}
