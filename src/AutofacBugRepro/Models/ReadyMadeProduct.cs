using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutofacBugRepro.Models
{
    [Table("Products")]
    public class ReadyMadeProduct : Product
    {
        [DisplayName("Url"), StringLength(100), Required]
        public string Url { get; set; }

        [DisplayName("Sku"), StringLength(50), Required]
        public string Sku { get; set; }
        
        [DisplayName("Product Name"), StringLength(100), Required]
        public string ProductName { get; set; }
        
        [DisplayName("Brand"), Required]
        public string Brand { get; set; }

        [DisplayName("Category"), Required]
        public int ProductCategoryId { get; set; }
        
        [DisplayName("Price"), Required]
        public decimal UnitPrice { get; set; }

        [DisplayName("MSRP"), Required]
        public decimal Msrp { get; set; }

        [StringLength(100), Required]
        public string Picture { get; set; }
        
        [DisplayName("Current Stock Level")]
        public int UnitInStock { get; set; }
    }
}
