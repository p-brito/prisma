namespace MagicDb.Core.Entities
{
    /// <summary>
    /// Defines the MagicDb options, where you can configure which provider you want to use.
    /// </summary>
    internal sealed class MagicDbOptions
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        public Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; }

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
