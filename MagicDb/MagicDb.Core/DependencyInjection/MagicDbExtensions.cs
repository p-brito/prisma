using MagicDb.Core.Entities;
using MagicDb.Core.Extensions;
using MagicDb.Core.Providers;

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

            if(services == null)
            {
                return null;
            }

            // Gets the options

            services
                .AddOptions()
                .AddOptionsSnapshot<MagicDbOptions>();


            MagicDbOptions configuration = services.BuildServiceProvider().GetRequiredService<MagicDbOptions>();

            // Adds the provider accordingly with the provided options

            switch (configuration.Provider)
            {
                case DbProvider.MongoDb:
                    services.AddMongoDbProvider(configuration);
                    break;
                default:
                    break;
            }

            return services;
        }

        #endregion

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

        #endregion
    }
}
