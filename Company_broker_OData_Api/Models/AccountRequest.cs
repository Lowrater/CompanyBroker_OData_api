namespace Company_broker_OData_Api.Models
{
    public class AccountRequest
    {
        public int CompanyId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
    }
}
