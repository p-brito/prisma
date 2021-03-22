using Prisma.Core.Entities;
using Prisma.Core.Exceptions;
using Prisma.Core.Extensions;
using Prisma.Core.Utils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace Prisma.Core.Providers
{
    /// <summary>
    /// Defines the CosmosDb provider.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Prisma.Core.IPrismaProvider{TEntity}" />
    internal sealed class CosmosDbProvider<TEntity> : IPrismaProvider<TEntity>
        where TEntity : BaseEntity
    {
        #region Private Fields

        private readonly IServiceProvider serviceProvider;

        private string container;

        #endregion

        #region Private Properties

        private ILogger<IPrismaProvider<TEntity>> Logger
        {
            get
            {
                return this.serviceProvider.GetRequiredService<ILogger<IPrismaProvider<TEntity>>>();
            }
        }

        private CosmosClient Client
        {
            get
            {
                return this.serviceProvider.GetRequiredService<CosmosClient>();
            }
        }

        private Database Database
        {
            get
            {
                return this.Client.GetDatabase(this.Configuration.GetValueOrDefault("DatabaseKey"));
            }
        }

        private PrismaOptions Options
        {
            get
            {
                return this.serviceProvider.GetRequiredService<PrismaOptions>();
            }
        }

        private ConfigString Configuration
        {
            get
            {
                return new ConfigString(this.Options.ConnectionString);
            }
        }

        private Container Container
        {
            get
            {
                return this.Database.GetContainer(this.container);
            }
        }

        #endregion

        #region Constructors        

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbProvider{TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public CosmosDbProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.InitializeAsync().GetAwaiter().GetResult();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            Validator.NotNullOrEmpty(() => id, id);

            try
            {
                this.Logger.LogInformation($"CosmosDb provider is trying to delete an entity with the id {id} from the collection {this.container}.");


                ItemResponse<TEntity> response = await this.Container.DeleteItemAsync<TEntity>(id, new PartitionKey(id), cancellationToken: cancellationToken).ConfigureAwait(false);
                
                if((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent) && response.Resource != null)
                {
                    return true;
                }

                return false;
            }
            catch (CosmosException ex)
            {
                this.Logger.LogError($"CosmosDb provider, an error occurred while trying to delete the entity with the id {id}.");

                throw new PrismaException(PrismaError.ErrorDelete, Properties.Resources.RES_Exception_Delete.Format(id), ex);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            Validator.NotNullOrEmpty(() => id, id);

            try
            {
                this.Logger.LogInformation($"CosmosDb provider is trying to get an entity with the id {id} from the collection {this.container}.");

                string queryString = $"SELECT * FROM {this.container} WHERE Id = '{id}'";

                FeedIterator<TEntity> queryable = this.Container.GetItemQueryIterator<TEntity>(queryString, null, null);

                if (queryable.HasMoreResults)
                {
                    FeedResponse<TEntity> queryResponse = await queryable.ReadNextAsync(cancellationToken).ConfigureAwait(false);

                    return queryResponse.Resource.First();
                }

                return null;

            }
            catch (CosmosException ex)
            {
                this.Logger.LogError($"CosmosDb provider, an error occurred while trying to get the entity with the id {id}.");

                throw new PrismaException(PrismaError.ErrorGet, Properties.Resources.RES_Exception_Get.Format(id), ex);
            }
           
        }

        /// <inheritdoc/>
        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Validator.NotNull(() => entity, entity);

            try
            {
                this.Logger.LogInformation($"CosmosDb provider is trying to insert a new entity with the id {entity.Id} into the Collection {this.container}.");

               
                ItemResponse<TEntity> response = await this.Container.CreateItemAsync(entity, new PartitionKey(entity.Id), cancellationToken: cancellationToken).ConfigureAwait(false);

                if ((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent) && response.Resource != null)
                {
                    return response.Resource;
                }

                throw new PrismaException(PrismaError.ErrorDelete, Properties.Resources.RES_Exception_Insert.Format(entity.Id));
            }
            catch(CosmosException ex)
            {
                this.Logger.LogError($"CosmosDb provider, an error occurred while trying to insert a new entity with the id {entity.Id}.");

                throw new PrismaException(PrismaError.ErrorInsert, Properties.Resources.RES_Exception_Insert.Format(entity.Id), ex);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Validator.NotNull(() => entity, entity);

            try
            {
                this.Logger.LogInformation($"CosmosDb provider is trying to update an entity with the id {entity.Id} from the collection {this.container}.");

                entity.UpdatedOn = DateTime.UtcNow;

                ItemResponse<TEntity> response = await this.Container.UpsertItemAsync(entity, new PartitionKey(entity.Id), cancellationToken: cancellationToken).ConfigureAwait(false);

                if ((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent) && response.Resource != null)
                {
                    return response.Resource;
                }

                throw new PrismaException(PrismaError.ErrorUpdate, Properties.Resources.RES_Exception_Update.Format(entity.Id));
            }
            catch(CosmosException ex)
            {
                this.Logger.LogError($"CosmosDb provider, an error occurred while trying to update the entity {entity.Id}.");

                throw new PrismaException(PrismaError.ErrorUpdate, Properties.Resources.RES_Exception_Update.Format(entity.Id), ex);
            }
        }

        #endregion

        #region Private Methods

        private async Task InitializeAsync()
        {
            this.Logger.LogInformation($"Initializing DynamoDb provider...");

            try
            {
                await this.CreateDatabaseIfNotExistsAsync().ConfigureAwait(false);

                await this.CreateContainerIfNotExistsAsync().ConfigureAwait(false);
            }
            catch(PrismaException ex)
            {
                throw new PrismaException(PrismaError.ErrorInitializeProvider, Properties.Resources.RES_Exception_Initializing_Provider.Format(this.Options.Provider.ToString()), ex);
            }
            catch
            {
                throw new PrismaException(PrismaError.ErrorInitializeProvider, Properties.Resources.RES_Exception_Initializing_Provider.Format(this.Options.Provider.ToString()));
            }
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            ThroughputProperties properties = this.BuildThroughputProperties();

            DatabaseResponse response = await this.Client.CreateDatabaseIfNotExistsAsync(this.Configuration.GetValueOrDefault("DatabaseKey"), properties).ConfigureAwait(false);

            if ((response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created) || response.Database == null)
            {
                throw new PrismaException(PrismaError.ErrorInitializeProvider, Properties.Resources.RES_Exception_Initializing_Resource.Format(nameof(Database)));
            }
        }

        private async Task CreateContainerIfNotExistsAsync()
        {
            this.container = $"{typeof(TEntity).Name}Container";

            ContainerProperties properties = new(id: this.container, partitionKeyPath: "/id");

            ThroughputProperties throughputProperties = this.BuildThroughputProperties();

            ContainerResponse response = await this.Database.CreateContainerIfNotExistsAsync(properties, throughputProperties).ConfigureAwait(false);

            if ((response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created) || response.Container == null)
            {
                throw new PrismaException(PrismaError.ErrorInitializeProvider, Properties.Resources.RES_Exception_Initializing_Resource.Format(nameof(Container)));
            }
        }

        private ThroughputProperties BuildThroughputProperties()
        {
            string throughput;

            ThroughputProperties properties = null;

            if (!string.IsNullOrEmpty(this.Configuration.GetValueOrDefault("AutoscaleThroughput")))
            {
                throughput = this.Configuration.GetValueOrDefault("AutoscaleThroughput");

                bool valid = int.TryParse(throughput, out int autoscaleThroughput);

                if (valid)
                {
                    properties = ThroughputProperties.CreateAutoscaleThroughput(autoscaleThroughput);

                    return properties;
                }
            }
            
            if (!string.IsNullOrEmpty(this.Configuration.GetValueOrDefault("ManualThroughput")))
            {
                throughput = this.Configuration.GetValueOrDefault("ManualThroughput");

                bool valid = int.TryParse(throughput, out int manualThroughput);

                if (valid)
                {
                    properties = ThroughputProperties.CreateManualThroughput(manualThroughput);

                    return properties;
                }
            }

            properties = ThroughputProperties.CreateAutoscaleThroughput(1000);

            return properties;
        }

        #endregion
    }
}
