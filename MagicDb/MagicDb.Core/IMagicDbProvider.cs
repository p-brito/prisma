using System.Threading;
using System.Threading.Tasks;

namespace MagicDb.Core
{
    /// <summary>
    /// Defines the MagicDb provider.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IMagicDbProvider<TEntity>
    {
        /// <summary>
        /// Gets the entity by the identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts the entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the entity asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    }
}
