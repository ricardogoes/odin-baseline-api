using MediatR;
using Odin.Baseline.Application.Customers.Common;

namespace Odin.Baseline.Application.Customers.CreateCustomer
{
    public class CreateCustomerInput : IRequest<CustomerOutput>
    {
        public string Name { get; private set; }
        public string Document { get; private set; }
        public string LoggedUsername { get; private set; }

        public CreateCustomerInput(string name, string document, string loggedUsername)
        {
            Name = name;
            Document = document;
            LoggedUsername = loggedUsername;
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
