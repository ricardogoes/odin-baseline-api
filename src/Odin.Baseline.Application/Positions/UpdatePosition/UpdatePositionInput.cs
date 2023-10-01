using MediatR;

namespace Odin.Baseline.Application.Positions.UpdatePosition
{
    public class UpdatePositionInput : IRequest<PositionOutput>
    {        
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }

        public UpdatePositionInput(Guid id, string name, decimal? baseSalary)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
        }

        public void ChangeId (Guid id)
        {
            Id = id;
        }
    }
}
