using System.ComponentModel.DataAnnotations;

namespace AutofacBugRepro.Models
{
    public abstract class Product : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
