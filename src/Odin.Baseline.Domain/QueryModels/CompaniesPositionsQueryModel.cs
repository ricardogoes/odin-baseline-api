namespace Odin.Baseline.Domain.QueryModels
{
    public class CompaniesPositionsQueryModel : ApiQueryModel
    {        
        public string Name { get; set; }
        public decimal? BaseSalary { get; set; }
        public bool? IsActive { get; set; }

        public CompaniesPositionsQueryModel(int pageNumber, int pageSize, string name, decimal? baseSalary, bool? isActive, string sort)
            : base(pageNumber, pageSize, sort)
        {
            Name = name;
            BaseSalary = baseSalary;
            IsActive = isActive;
        }
    }
}
