using Amazon.DynamoDBv2;
using Prisma.Core.Entities;
using Prisma.Core.Extensions;
using Prisma.Core.Providers;
using Prisma.Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MongoDB.Driver;

namespace Prisma.Core.DependencyInjection
{
    /// <summary>
    /// Defines the Prisma extensions class.
    /// </summary>
    public static class PrismaExtensions
    {
        #region Public Methods

        /// <summary>
        /// Adds the Prisma service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddPrisma(this IServiceCollection services)
        {
            // Validate

            if (services == null)
            {
                return null;
            }

            // Gets the options

            services
                .AddOptions()
                .AddOptionsSnapshot<PrismaOptions>();

            services.AddLogging();

            PrismaOptions configuration = services.BuildServiceProvider().GetRequiredService<PrismaOptions>();

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

        private static IServiceCollection AddMongoDbProvider(this IServiceCollection services, PrismaOptions configuration)
        {
            services.TryAddSingleton<IMongoClient>(c =>
            {
                return new MongoClient(configuration.ConnectionString);
            });

            services.TryAddTransient(typeof(IPrismaProvider<>), typeof(MongoDbProvider<>));

            return services;
        }

        private static IServiceCollection AddDynamoDbProvider(this IServiceCollection services, PrismaOptions configuration)
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

            services.TryAddTransient(typeof(IPrismaProvider<>), typeof(DynamoDbProvider<>));

            return services;
        }

        #endregion Private Methods
    }
}