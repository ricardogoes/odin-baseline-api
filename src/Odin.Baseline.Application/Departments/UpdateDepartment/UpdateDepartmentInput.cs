using MediatR;

namespace Odin.Baseline.Application.Departments.UpdateDepartment
{
    public class UpdateDepartmentInput : IRequest<DepartmentOutput>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public UpdateDepartmentInput(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public void ChangeId(Guid id)
        {
            Id = id;
        }
    }
}
