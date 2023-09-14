using MediatR;
using Odin.Baseline.Application.Departments.Common;

namespace Odin.Baseline.Application.Departments.CreateDepartment
{
    public class CreateDepartmentInput : IRequest<DepartmentOutput>
    {   

        public Guid CustomerId { get; private set; }
        public string Name { get; private set; }
        public string LoggedUsername { get; private set; }

        public CreateDepartmentInput(Guid customerId, string name, string loggedUsername)
        {
            CustomerId = customerId;
            Name = name;
            LoggedUsername = loggedUsername;
        }

        public void ChangeLoggedUsername(string username)
        {
            LoggedUsername = username;
        }
    }
}
