using System.ComponentModel.DataAnnotations;

namespace Odin.Baseline.Domain.ViewModels.CompaniesPositions
{
    public class CompanyPositionToUpdate
    {
        [Key]
        [Required(ErrorMessage = "PositionId required")]
        public int PositionId { get; set; }

        [Required(ErrorMessage = "CustomerId required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        public decimal? BaseSalary { get; set; }
    }
}
