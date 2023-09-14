using MediatR;
using Odin.Baseline.Application.Positions.Common;

namespace Odin.Baseline.Application.Positions.CreatePosition
{
    public class CreatePositionInput : IRequest<PositionOutput>
    {        
        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }
        public string LoggedUsername { get; private set; }

        public CreatePositionInput(Guid customerId, string name, decimal? baseSalary, string loggedUsername)
        {
            CustomerId = customerId;
            Name = name;
            BaseSalary = baseSalary;
            LoggedUsername = loggedUsername;
        }

        public void ChangeLoggedUsername(string username)
        {
            LoggedUsername = username;
        }

    }
}
