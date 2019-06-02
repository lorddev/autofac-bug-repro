namespace AutofacBugRepro.Models
{
    public class InventoryProductSummary
    {
        public string Sku { get; set; }
        public string Htm { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int UnitInStock { get; set; }
        public bool IsAvailable { get; set; }
        public string SupplierProductId { get; set; }
    }
}
