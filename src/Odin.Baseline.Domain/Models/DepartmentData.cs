namespace Odin.Baseline.Domain.DTO
{
    public class DepartmentData
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public DepartmentData(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
