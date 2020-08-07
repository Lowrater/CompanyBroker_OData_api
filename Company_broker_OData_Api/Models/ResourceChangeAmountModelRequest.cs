namespace Company_broker_OData_Api.Models
{
    public class ResourceChangeAmountModelRequest
    {
        public bool IncreaseAmount { get; set; }
        public int companyId { get; set; }
        public int resourceId { get; set; }
    }
}
