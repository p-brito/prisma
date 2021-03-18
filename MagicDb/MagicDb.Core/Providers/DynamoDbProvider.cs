
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MagicDb.Core.Entities;
using MagicDb.Core.Exceptions;
using MagicDb.Core.Extensions;
using MagicDb.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MagicDb.Core.Providers
{
    internal sealed class DynamoDbProvider<TEntity> : IMagicDbProvider<TEntity>
        where TEntity : BaseEntity
    {
        #region Private Properties

        private readonly string TableName;

        private readonly IServiceProvider serviceProvider;

        #endregion

        #region Private Properties

        private ILogger<IMagicDbProvider<TEntity>> Logger
        {
            get
            {
                return this.serviceProvider.GetRequiredService<ILogger<IMagicDbProvider<TEntity>>>();
            }
        }

        private IAmazonDynamoDB Client
        {
            get
            {
                return this.serviceProvider.GetRequiredService<IAmazonDynamoDB>();
            }
        }

        private IDynamoDBContext Context
        {
            get
            {
                return new DynamoDBContext(this.Client);
            }
        }

        private MagicDbOptions Options
        {
            get
            {
                return this.serviceProvider.GetRequiredService<MagicDbOptions>();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDbProvider{TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public DynamoDbProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            this.Logger.LogInformation($"Initializing DynamoDb provider...");

            this.TableName = $"{typeof(TEntity).Name}Collection";

            InitializeAsync().GetAwaiter().GetResult();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            Validator.NotNullOrEmpty(() => id, id);

            try
            {
                this.Logger.LogInformation($"DynamoDb provider is trying to delete an entity with the id {id} from the table {this.TableName}.");

                await this.Context.DeleteAsync<TEntity>(id, new DynamoDBOperationConfig
                {
                    OverrideTableName = this.TableName
                }, cancellationToken).ConfigureAwait(false);

                return true;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.Logger.LogError($"DynamoDb provider, an error occurred while trying to delete the entity with the id {id}.");

                throw new MagicDbException(MagicDbError.ErrorDelete, Properties.Resources.RES_Exception_Delete.Format(id), ex);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            Validator.NotNullOrEmpty(() => id, id);

            try
            {
                this.Logger.LogInformation($"DynamoDb provider is trying to get an entity with the id {id} from the table {this.TableName}.");

                List<ScanCondition> scanConditions = new()
                {
                    new ScanCondition("Id", ScanOperator.Equal, id)
                };

                IEnumerable<TEntity> result = await this.Context.ScanAsync<TEntity>(scanConditions, 
                    new DynamoDBOperationConfig
                    {
                        OverrideTableName = this.TableName
                    }).GetRemainingAsync(cancellationToken).ConfigureAwait(false);
                

                if (result != null)
                {
                    return result.FirstOrDefault();
                }

                return null;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.Logger.LogError($"DynamoDb provider, an error occurred while trying to get the entity with the id {id}.");

                throw new MagicDbException(MagicDbError.ErrorGet, Properties.Resources.RES_Exception_Get.Format(id), ex);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Validator.NotNull(() => entity, entity);

            try
            {
                this.Logger.LogInformation($"DynamoDb provider is trying to insert a new entity with the id {entity.Id} into the table {this.TableName}.");

                await this.Context.SaveAsync(entity, 
                                             new DynamoDBOperationConfig 
                                             { 
                                                 OverrideTableName = this.TableName 
                                             }, cancellationToken).ConfigureAwait(false);

                return entity;
            }
            catch(AmazonDynamoDBException ex)
            {
                this.Logger.LogError($"DynamoDb provider, an error occurred while trying to insert a new entity with the id {entity.Id}.");

                throw new MagicDbException(MagicDbError.ErrorInsert, Properties.Resources.RES_Exception_Insert.Format(entity.Id), ex);
            }
        }

        /// <inheritdoc/>
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                this.Logger.LogInformation($"DynamoDb provider is trying to update an entity with the id {entity.Id} from the table {this.TableName}.");

                // Retrieve the record

                TEntity loadedEntity = await this.Context.LoadAsync<TEntity>(entity.Id, new DynamoDBOperationConfig
                {
                    OverrideTableName = this.TableName
                }, cancellationToken).ConfigureAwait(false);

                if (loadedEntity == null)
                {
                    throw new MagicDbException(MagicDbError.ErrorUpdate, Properties.Resources.RES_Exception_Update_EntityDoesNotExist.Format(entity.Id));
                }

                await this.Context.SaveAsync(entity,
                                           new DynamoDBOperationConfig
                                           {
                                               OverrideTableName = this.TableName
                                           }, cancellationToken).ConfigureAwait(false);

                return entity;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.Logger.LogError($"DynamoDb provider, an error occurred while trying to update the entity {entity.Id}.");

                throw new MagicDbException(MagicDbError.ErrorUpdate, Properties.Resources.RES_Exception_Update.Format(entity.Id), ex);
            }
        }

        #endregion

        #region Private Methods

        private async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ListTablesResponse tables = await this.Client.ListTablesAsync(cancellationToken).ConfigureAwait(false);

                if (tables == null || !tables.TableNames.Any(t => t == this.TableName))
                {
                    this.Logger.LogInformation($"DynamoDb provider is trying to create the table {this.TableName}.");

                    await CreateTableAsync(cancellationToken).ConfigureAwait(false);

                    await VerifyTableStatusAsync(cancellationToken).ConfigureAwait(false);

                    this.Logger.LogInformation($"DynamoDb provider table {this.TableName} is ready.");
                }
            }
            catch
            {
                this.Logger.LogError($"DynamoDb provider, an error occurred while initializing the dynamoDb provider.");

                throw new MagicDbException(MagicDbError.ErrorInitializeProvider, Properties.Resources.RES_Exception_Initializing_Provider.Format(this.Options.Provider.ToString()));
            }
        }

        private async Task CreateTableAsync(CancellationToken cancellationToken = default)
        {
            // Created the table metadata

            CreateTableRequest request = new()
            {
                TableName = this.TableName,
                KeySchema = AddKeySchema(),
                AttributeDefinitions = AddAttributeDefinitions(),
                ProvisionedThroughput = AddThroughputValues()
            };

            // Creates the table

            CreateTableResponse response = await this.Client.CreateTableAsync(request, cancellationToken).ConfigureAwait(false);

            if(response.HttpStatusCode != System.Net.HttpStatusCode.Created && response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new MagicDbException();
            }
        }

        private async Task VerifyTableStatusAsync(CancellationToken cancellationToken = default)
        {
            TableStatus status = "";

            do
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

                try
                {
                    DescribeTableResponse response = await this.Client.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = this.TableName
                    }, cancellationToken).ConfigureAwait(false);

                    status = response.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                   this.Logger.LogError($"DynamoDb provider, an error occurred while trying to verify if the table is ready.");
                }

            } while (status != TableStatus.ACTIVE);
        }

        private static ProvisionedThroughput AddThroughputValues()
        {
            return new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            };
        }

        private static List<KeySchemaElement> AddKeySchema()
        {
            return new List<KeySchemaElement> {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH" // Partition key
                    }
                };
        }

        private static List<AttributeDefinition> AddAttributeDefinitions()
        {
            List<AttributeDefinition> definitions = new();

            definitions.Add(new AttributeDefinition
            {
                AttributeName = "Id",
                AttributeType = ScalarAttributeType.S
            });

            return definitions;
        }

        private static string GetAttributeType(Type type)
        {
            if(type == typeof(string) || type == typeof(DateTime))
            {
                return ScalarAttributeType.S;
            }

            if (type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(decimal))
            {
                return ScalarAttributeType.N;
            }

            return ScalarAttributeType.B;
        }

        #endregion
    }
}
