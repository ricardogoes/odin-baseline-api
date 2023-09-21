namespace Odin.Baseline.Api.Models.Positions
{
    public class CreatePositionApiRequest
    {
        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }

        public CreatePositionApiRequest(Guid customerId, string name, decimal? baseSalary)
        {
            CustomerId = customerId;
            Name = name;
            BaseSalary = baseSalary;
        }

    }
}
