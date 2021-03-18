using Amazon.DynamoDBv2;
using MagicDb.Core.Entities;
using MagicDb.Core.Extensions;
using MagicDb.Core.Providers;
using MagicDb.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MongoDB.Driver;

namespace MagicDb.Core.DependencyInjection
{
    /// <summary>
    /// Defines the MagicDb extensions class.
    /// </summary>
    public static class MagicDbExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds the MagicDb service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddMagicDb(this IServiceCollection services)
        {
            // Validate

            if (services == null)
            {
                return null;
            }

            // Gets the options

            services
                .AddOptions()
                .AddOptionsSnapshot<MagicDbOptions>();

            services.AddLogging();

            MagicDbOptions configuration = services.BuildServiceProvider().GetRequiredService<MagicDbOptions>();

            // Adds the provider accordingly with the provided options

            switch (configuration.Provider)
            {
                case DbProvider.MongoDb:
                    services.AddMongoDbProvider(configuration);
                    break;
                case DbProvider.DynamoDb:
                    services.AddDynamoDbProvider(configuration);
                    break;

                default:
                    break;
            }

            return services;
        }

        #endregion Public Methods

        #region Private Methods

        private static IServiceCollection AddMongoDbProvider(this IServiceCollection services, MagicDbOptions configuration)
        {
            services.TryAddSingleton<IMongoClient>(c =>
            {
                return new MongoClient(configuration.ConnectionString);
            });

            services.TryAddTransient(typeof(IMagicDbProvider<>), typeof(MongoDbProvider<>));

            return services;
        }

        private static IServiceCollection AddDynamoDbProvider(this IServiceCollection services, MagicDbOptions configuration)
        {
            ConfigString dynamoDbConfig = new(configuration.ConnectionString);

            services.TryAddSingleton<IAmazonDynamoDB>(c =>
            {
                return new AmazonDynamoDBClient(
                    new Amazon.Runtime.BasicAWSCredentials(
                        dynamoDbConfig.GetValueOrDefault("accesskey"),
                        dynamoDbConfig.GetValueOrDefault("secretkey")),
                    Amazon.RegionEndpoint.GetBySystemName(dynamoDbConfig.GetValueOrDefault("region")));
            });

            services.TryAddTransient(typeof(IMagicDbProvider<>), typeof(DynamoDbProvider<>));

            return services;
        }

        #endregion Private Methods
    }
}