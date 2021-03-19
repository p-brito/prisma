using System;

namespace Prisma.Core.Exceptions
{
    /// <summary>
    /// Defines the Prisma exception class.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class PrismaException : Exception
    {
        #region Public Properties

        /// <summary>
        /// Gets the code of the error that caused the exception.
        /// </summary>
        public PrismaError ErrorCode
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaException"/> class.
        /// </summary>
        public PrismaException()
            : base()
        {
            this.ErrorCode = PrismaError.Unspecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PrismaException(string message)
            : base(message)
        {
            this.ErrorCode = PrismaError.Unspecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public PrismaException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = PrismaError.Unspecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaException"/> class.
        /// </summary>
        /// <param name="error">The code of the error that caused the exception.</param>
        public PrismaException(PrismaError error)
            : base()
        {
            this.ErrorCode = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaException"/> class.
        /// </summary>
        /// <param name="error">The code of the error that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public PrismaException(PrismaError error, string message)
            : base(message)
        {
            this.ErrorCode = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaException"/> class.
        /// </summary>
        /// <param name="error">The code of the error that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public PrismaException(PrismaError error, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = error;
        }

        #endregion
    }
}
