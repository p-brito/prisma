namespace MagicDb.Core.Entities
{
    /// <summary>
    /// Defines the MagicDb options, where you can configure which provider you want to use.
    /// </summary>
    public sealed class MagicDbOptions
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        public DbProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicDbOptions"/> class.
        /// </summary>
        public MagicDbOptions()
        {
        }

        #endregion
    }
}
