using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutofacBugRepro.Models
{
    [Table("tbl_product_type")]
    public class InventoryProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Column("ProductName")]
        public string Name { get; set; }
    }
}
