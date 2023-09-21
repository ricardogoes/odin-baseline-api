namespace Odin.Baseline.Api.Models.Customers
{
    public class CreateCustomerApiRequest
    {
        public string Name { get; private set; }
        public string Document { get; private set; }

        public CreateCustomerApiRequest(string name, string document)
        {
            Name = name;
            Document = document;
        }
    }
}
