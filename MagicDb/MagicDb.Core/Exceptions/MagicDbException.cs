using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicDb.Core.Exceptions
{
    /// <summary>
    /// Defines the MagicDb exception class.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class MagicDbException : Exception
    {
        #region Public Properties

        /// <summary>
        /// Gets the code of the error that caused the exception.
        /// </summary>
        public MagicDbError ErrorCode
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicDbException"/> class.
        /// </summary>
        public MagicDbException()
            : base()
        {
            this.ErrorCode = MagicDbError.Unspecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicDbException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MagicDbException(string message)
            : base(message)
        {
            this.ErrorCode = MagicDbError.Unspecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicDbException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public MagicDbException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = MagicDbError.Unspecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicDbException"/> class.
        /// </summary>
        /// <param name="error">The code of the error that caused the exception.</param>
        public MagicDbException(MagicDbError error)
            : base()
        {
            this.ErrorCode = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicDbException"/> class.
        /// </summary>
        /// <param name="error">The code of the error that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public MagicDbException(MagicDbError error, string message)
            : base(message)
        {
            this.ErrorCode = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicDbException"/> class.
        /// </summary>
        /// <param name="error">The code of the error that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public MagicDbException(MagicDbError error, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = error;
        }

        #endregion
    }
}
