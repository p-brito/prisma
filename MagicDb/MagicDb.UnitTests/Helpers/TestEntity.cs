using MagicDb.Core.Entities;

namespace MagicDb.UnitTests.Helpers
{
    /// <summary>
    /// Defines the test entity.
    /// </summary>
    /// <seealso cref="MagicDb.Core.Entities.BaseEntity" />
    public sealed class TestEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
