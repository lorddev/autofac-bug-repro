namespace AutofacBugRepro.Models
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        int Id { get; set; }
    }
}

