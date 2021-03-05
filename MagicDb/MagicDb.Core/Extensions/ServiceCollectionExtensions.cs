using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MagicDb.Core.Extensions
{
    /// <summary>
    /// Defines the service collection extensions class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the options snapshot to the service collection.
        /// </summary>
        /// <typeparam name="TOptions">The type of the options.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddOptionsSnapshot<TOptions>(this IServiceCollection services)
            where TOptions : class, new()
        {
            if (services == null)
            {
                return null;
            }

            services.AddOptions()
                    .AddTransient((provider) =>
                    {
                        using IServiceScope scope = provider.CreateScope();
                        return scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<TOptions>>().Value;
                    });

            return services;
        }
    }
}
