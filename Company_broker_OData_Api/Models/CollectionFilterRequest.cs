namespace Company_broker_OData_Api.Models
{
    public class CollectionFilterRequest
    {
        public int[] CompanyChoices { get; set; }
        public string[] ProductTypeChoices { get; set; }
        public string[] ProductNameChoices { get; set; }
        public string SearchWord { get; set; }
        public bool Partners_OnlyChoice { get; set; }
        public bool ResourceActive { get; set; }
        public decimal LowestPriceChoice { get; set; }
        public decimal HigestPriceChoice { get; set; }
    }
}
