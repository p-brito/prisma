﻿namespace Prisma.Core.Exceptions
{
    public enum PrismaError
    {
        /// <summary>
        /// Unspecified error.
        /// </summary>
        Unspecified,

        /// <summary>
        /// An error occurred while trying to delete the specified entity.
        /// </summary>
        ErrorDelete,

        /// <summary>
        /// An error occurred while trying to get the specified entity.
        /// </summary>
        ErrorGet,

        /// <summary>
        /// An error occurred while trying to insert the specified entity.
        /// </summary>
        ErrorInsert,

        /// <summary>
        /// An error occurred while trying to update the specified entity.
        /// </summary>
        ErrorUpdate,

        /// <summary>
        /// An error occurred while trying to initialize the provider.
        /// </summary>
        ErrorInitializeProvider
    }
}
