using MagicDb.Core.Entities;
using MagicDb.Core.Exceptions;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MagicDb.Core.Providers
{
    /// <summary>
    /// Defines the MongoDb provider.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="MagicDb.Core.IMagicDbProvider{TEntity}" />
    internal sealed class MongoDbProvider<TEntity> : IMagicDbProvider<TEntity>
        where TEntity : BaseEntity
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;

        private string collection;

        private readonly IClientSessionHandle clientSessionHandle;

        #endregion

        #region Private Properties
        private IMongoClient Client
        {
            get
            {
                return this.serviceProvider.GetRequiredService<IMongoClient>();
            }
        }

        private IMongoDatabase Database
        {
            get
            {
                return this.Client.GetDatabase(this.Configuration.DatabaseName);
            }
        }

        private MagicDbOptions Configuration
        {
            get
            {
                return this.serviceProvider.GetRequiredService<MagicDbOptions>();
            }
        }

        private IMongoCollection<TEntity> Collection
        {
            get
            {
                return this.Database.GetCollection<TEntity>(collection);
            }
        }

        #endregion

        #region Constructors        

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbProvider{TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public MongoDbProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.clientSessionHandle = this.Client.StartSession();

            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                DeleteResult result = await this.Collection.DeleteOneAsync(clientSessionHandle, f => f.Id == entity.Id, cancellationToken: cancellationToken).ConfigureAwait(false);

                return result.IsAcknowledged;
            }
            catch
            {
                throw new MagicDbException(MagicDbError.ErrorDelete, Properties.Resources.RES_Exception_Delete);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                IAsyncCursor<TEntity> entity = await this.Collection.FindAsync(e => e.Id == id, cancellationToken: cancellationToken).ConfigureAwait(false);

                return entity.First(cancellationToken);
            }
            catch
            {
                throw new MagicDbException(MagicDbError.ErrorGet, Properties.Resources.RES_Exception_Get);
            }
           
        }

        /// <inheritdoc/>
        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.Collection.InsertOneAsync(clientSessionHandle, entity, cancellationToken: cancellationToken).ConfigureAwait(false);

                return entity;
            }
            catch
            {
                throw new MagicDbException(MagicDbError.ErrorInsert, Properties.Resources.RES_Exception_Insert);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                Expression<Func<TEntity, string>> func = f => f.Id;

                string value = (string)entity.GetType().GetProperty(func.Body.ToString().Split(".")[1]).GetValue(entity, null);

                FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(func, value);

                if (entity != null)
                {
                    entity.UpdatedOn = DateTime.UtcNow;

                    await this.Collection.ReplaceOneAsync(clientSessionHandle, filter, entity, cancellationToken: cancellationToken).ConfigureAwait(false);
                }

                return entity;

            }
            catch
            {
                throw new MagicDbException(MagicDbError.ErrorUpdate, Properties.Resources.RES_Exception_Update);
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            this.collection = $"{typeof(TEntity).Name}s";

            if (!this.Database.ListCollectionNames().ToList().Contains(collection))
            {
                this.Database.CreateCollection(collection);
            }
        }

        #endregion
    }
}
