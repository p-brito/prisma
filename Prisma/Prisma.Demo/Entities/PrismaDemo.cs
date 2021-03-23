using Prisma.Core.Entities;

namespace Prisma.Demo.Entities
{
    /// <summary>
    /// Defines the Demo entity.
    /// </summary>
    /// <seealso cref="Prisma.Core.Entities.BaseEntity" />
    public class PrismaDemo : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
