namespace Odin.Baseline.Api.Models.Positions
{
    public class UpdatePositionApiRequest
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }

        public UpdatePositionApiRequest(Guid id, Guid customerId, string name, decimal? baseSalary)
        {
            Id = id;
            CustomerId = customerId;
            Name = name;
            BaseSalary = baseSalary;
        }
    }
}
