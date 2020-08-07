using CompanyBroker_DBS;

namespace Company_broker_OData_Api.Models
{
    public class CompanyResponse
    {
        public CompanyResponse(Company company)
        {
            Id = company.CompanyId;
            Name = company.CompanyName;
            Active = company.Active;
            Balance = company.CompanyBalance;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public decimal Balance { get; set; }


    }
}
