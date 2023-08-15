using System.ComponentModel.DataAnnotations;

namespace Odin.Baseline.Domain.ViewModels.Customers
{
    public class CustomerToInsert
    {
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        public string Document { get; set; }
    }
}
