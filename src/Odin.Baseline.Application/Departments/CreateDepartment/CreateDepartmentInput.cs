using MediatR;

namespace Odin.Baseline.Application.Departments.CreateDepartment
{
    public class CreateDepartmentInput : IRequest<DepartmentOutput>
    {   
        public string Name { get; private set; }

        public CreateDepartmentInput(string name)
        {
            Name = name;
        }
    }
}
