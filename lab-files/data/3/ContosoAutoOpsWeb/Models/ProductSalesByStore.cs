namespace ContosoAutoOpsWeb.Models
{
    public class ProductSalesByStore
    {
        public string Store { get; set; }
        public string Product { get; set; }
        public decimal SalesTotal { get; set; }
    }
}