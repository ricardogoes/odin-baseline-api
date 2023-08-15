namespace Odin.Baseline.Domain.QueryModels
{
    public class EmployeesQueryModel : ApiQueryModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal? Salary { get; set; }
        public bool? IsActive { get; set; }

        public EmployeesQueryModel(int pageNumber, int pageSize, string firstName, string lastName, string email, decimal? salary, bool? isActive, string sort)
            : base(pageNumber, pageSize, sort)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Salary = salary;
            IsActive = isActive;
        }
    }
}
