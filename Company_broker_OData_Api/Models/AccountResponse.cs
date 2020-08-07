using CompanyBroker_DBS;

namespace Company_broker_OData_Api.Models
{
    public class AccountResponse
    {
        public AccountResponse(CompanyAccount account)
        {
            CompanyId = account.CompanyId;
            Email = account.Email;
            Username = account.Username;
            Active = account.Active;
            UserId = account.UserId;
        }

        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }


    }
}
