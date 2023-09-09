namespace Odin.Baseline.Domain.DTO
{
    public class CustomerData
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public CustomerData(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
