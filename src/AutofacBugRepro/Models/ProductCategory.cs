using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AutofacBugRepro.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Product Category Object
    /// </summary>
    public class ProductCategory : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Product Category Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parent Product Category Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Parent Product Category
        /// </summary>
        [ForeignKey("ParentId")]
        public ProductCategory Parent { get; set; }

        public string Url { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// Children Product Categories
        /// </summary>
        public virtual ICollection<ProductCategory> Children { get; set; }

        /// <summary>
        /// Has Children
        /// </summary>
        [NotMapped]
        public bool HasChildren => Children != null && Children.Any();
        
        public ICollection<ReadyMadeProduct> Products { get; set; }
    }
}
