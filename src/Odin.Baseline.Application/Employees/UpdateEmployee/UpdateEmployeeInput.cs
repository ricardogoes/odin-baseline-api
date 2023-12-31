﻿using MediatR;

namespace Odin.Baseline.Application.Employees.UpdateEmployee
{
    public class UpdateEmployeeInput : IRequest<EmployeeOutput>
    {       
        public Guid Id { get; private set; }
        public Guid? DepartmentId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }

        public UpdateEmployeeInput(Guid id, string firstName, string lastName, string document, string email, Guid? departmentId = null)
        {
            Id = id;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Email = email;
        }

        public void ChangeId(Guid id)
        {
            Id = id;
        }

        public void ChangeDocument(string document)
        {
            Document = document;
        }
    }
}
