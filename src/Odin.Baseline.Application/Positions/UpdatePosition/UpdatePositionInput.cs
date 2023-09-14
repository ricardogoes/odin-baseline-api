using MediatR;
using Odin.Baseline.Application.Positions.Common;

namespace Odin.Baseline.Application.Positions.UpdatePosition
{
    public class UpdatePositionInput : IRequest<PositionOutput>
    {        
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public decimal? BaseSalary { get; private set; }
        public string LoggedUsername { get; private set; }

        public UpdatePositionInput(Guid id, Guid customerId, string name, decimal? baseSalary, string loggedUsername)
        {
            Id = id;
            CustomerId = customerId;
            Name = name;
            BaseSalary = baseSalary;
            LoggedUsername = loggedUsername;
        }

        public void ChangeId (Guid id)
        {
            Id = id;
        }

        public void ChangeLoggedUsername(string username)
        {
            LoggedUsername = username;
        }

    }
}
