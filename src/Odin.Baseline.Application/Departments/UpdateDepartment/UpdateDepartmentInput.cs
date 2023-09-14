using MediatR;
using Odin.Baseline.Application.Departments.Common;

namespace Odin.Baseline.Application.Departments.UpdateDepartment
{
    public class UpdateDepartmentInput : IRequest<DepartmentOutput>
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public string LoggedUsername { get; private set; }

        public UpdateDepartmentInput(Guid id, Guid customerId, string name, string loggedUsername)
        {
            Id = id;
            CustomerId = customerId;
            Name = name;
            LoggedUsername = loggedUsername;
        }

        public void ChangeId(Guid id)
        {
            Id = id;
        }

        public void ChangeLoggedUsername(string username)
        {
            LoggedUsername = username;
        }
    }
}
