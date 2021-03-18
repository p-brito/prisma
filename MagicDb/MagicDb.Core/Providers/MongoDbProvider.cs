using MagicDb.Core.Entities;
using MagicDb.Core.Exceptions;
using MagicDb.Core.Extensions;
using MagicDb.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        private ILogger<IMagicDbProvider<TEntity>> Logger
        {
            get
            {
                return this.serviceProvider.GetRequiredService<ILogger<IMagicDbProvider<TEntity>>>();
            }
        }

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
                string databaseName = MongoUrl.Create(this.Options.ConnectionString).DatabaseName;

                return this.Client.GetDatabase(databaseName);
            }
        }

        private MagicDbOptions Options
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
        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            Validator.NotNullOrEmpty(() => id, id);

            try
            {
                this.Logger.LogInformation($"MongoDb provider is trying to delete an entity with the id {id} from the collection {this.collection}.");

                DeleteResult result = await this.Collection.DeleteOneAsync(f => f.Id == id, cancellationToken: cancellationToken).ConfigureAwait(false);

                return result.IsAcknowledged;
            }
            catch (MongoException ex)
            {
                this.Logger.LogError($"MongoDb provider, an error occurred while trying to delete the entity with the id {id}.");

                throw new MagicDbException(MagicDbError.ErrorDelete, Properties.Resources.RES_Exception_Delete.Format(id), ex);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            Validator.NotNullOrEmpty(() => id, id);

            try
            {
                this.Logger.LogInformation($"MongoDb provider is trying to get an entity with the id {id} from the collection {this.collection}.");

                IAsyncCursor<TEntity> entity = await this.Collection.FindAsync(e => e.Id == id, cancellationToken: cancellationToken).ConfigureAwait(false);

                return entity.FirstOrDefault(cancellationToken);
            }
            catch (MongoException ex)
            {
                this.Logger.LogError($"MongoDb provider, an error occurred while trying to get the entity with the id {id}.");

                throw new MagicDbException(MagicDbError.ErrorGet, Properties.Resources.RES_Exception_Get.Format(id), ex);
            }
           
        }

        /// <inheritdoc/>
        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Validator.NotNull(() => entity, entity);

            try
            {
                this.Logger.LogInformation($"MongoDb provider is trying to insert a new entity with the id {entity.Id} into the Collection {this.collection}.");

                await this.Collection.InsertOneAsync(clientSessionHandle, entity, cancellationToken: cancellationToken).ConfigureAwait(false);

                return entity;
            }
            catch(MongoException ex)
            {
                this.Logger.LogError($"MongoDb provider, an error occurred while trying to insert a new entity with the id {entity.Id}.");

                throw new MagicDbException(MagicDbError.ErrorInsert, Properties.Resources.RES_Exception_Insert.Format(entity.Id), ex);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Validator.NotNull(() => entity, entity);

            try
            {
                this.Logger.LogInformation($"MongoDb provider is trying to update an entity with the id {entity.Id} from the collection {this.collection}.");

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
            catch(MongoException ex)
            {
                this.Logger.LogError($"MongoDb provider, an error occurred while trying to update the entity {entity.Id}.");

                throw new MagicDbException(MagicDbError.ErrorUpdate, Properties.Resources.RES_Exception_Update.Format(entity.Id), ex);
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            this.Logger.LogInformation($"Initializing DynamoDb provider...");

            try
            {
                this.collection = $"{typeof(TEntity).Name}Collection";

                if (!this.Database.ListCollectionNames().ToList().Contains(collection))
                {
                    this.Database.CreateCollection(collection);
                }
            }
            catch
            {
                throw new MagicDbException(MagicDbError.ErrorInitializeProvider, Properties.Resources.RES_Exception_Initializing_Provider.Format(this.Options.Provider.ToString()));
            }
        }

        #endregion
    }
}
