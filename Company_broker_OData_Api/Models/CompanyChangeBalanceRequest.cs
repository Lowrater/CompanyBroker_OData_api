namespace Company_broker_OData_Api.Models
{
    public class CompanyChangeBalanceRequest
    {
        public int companyId { get; set; }
        public decimal price { get; set; }
        public bool increaseBalance { get; set; }
    }
}
