using MediatR;
using Odin.Baseline.Application.Customers.Common;

namespace Odin.Baseline.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerInput : IRequest<CustomerOutput>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }
        public string LoggedUsername { get; private set; }

        public UpdateCustomerInput(Guid id, string name, string document, string loggedUsername)
        {
            Id = id;
            Name = name;
            Document = document;
            LoggedUsername = loggedUsername;
        }

        public void ChangeId(Guid id)
        {
            Id = id;
        }

        public void ChangeDocument(string document)
        {
            Document = document;
        }

        public void ChangeLoggedUsername(string username)
        {
            LoggedUsername = username;
        }
    }
}
