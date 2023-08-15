using System.ComponentModel.DataAnnotations;

namespace Odin.Baseline.Domain.ViewModels.Customers
{
    public class CustomerToUpdate
    {
        [Key]
        [Required(ErrorMessage = "CustomerId required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        public string Document { get; set; }
    }
}
