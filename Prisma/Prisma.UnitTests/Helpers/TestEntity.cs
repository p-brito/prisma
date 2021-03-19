using Prisma.Core.Entities;

namespace Prisma.UnitTests.Helpers
{
    /// <summary>
    /// Defines the test entity.
    /// </summary>
    /// <seealso cref="Prisma.Core.Entities.BaseEntity" />
    public sealed class TestEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
