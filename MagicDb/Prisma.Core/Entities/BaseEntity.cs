using System;

namespace Prisma.Core.Entities
{
    /// <summary>
    /// Defines the base entity class. 
    /// </summary>
    /// <remarks>
    /// To use this service all entities should inherit this.
    /// </remarks>
    public class BaseEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the updated on.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        public BaseEntity()
        {
            this.Id = Guid.NewGuid().ToString();

            this.CreatedOn = DateTime.UtcNow;

            this.UpdatedOn = null;
        }

        #endregion
    }
}
