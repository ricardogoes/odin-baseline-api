using MediatR;

namespace Odin.Baseline.Application.Positions.CreatePosition
{
    public class CreatePositionInput : IRequest<PositionOutput>
    {        
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }

        public CreatePositionInput(string name, decimal? baseSalary)
        {
            Name = name;
            BaseSalary = baseSalary;
        }
    }
}
