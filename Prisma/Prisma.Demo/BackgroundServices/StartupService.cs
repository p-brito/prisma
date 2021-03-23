using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prisma.Core;
using Prisma.Demo.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prisma.Demo.BackgroundServices
{
    public sealed class StartupService : BackgroundService
    {

        #region Private Fields

        private readonly IServiceProvider serviceProvider;

        #endregion

        #region Private Properties

        private IPrismaProvider<PrismaDemo> provider
        {
            get
            {
                return this.serviceProvider.GetRequiredService<IPrismaProvider<PrismaDemo>>();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public StartupService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method is called when the <see cref="IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="CancellationToken)" /> is called.</param>
        /// <returns>
        /// A <see cref="Task" /> that represents the long running operations.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PrismaDemo demo = new();

            PrismaDemo insert = await provider.InsertAsync(demo, stoppingToken).ConfigureAwait(false);

            PrismaDemo get = await provider.GetAsync(insert.Id, stoppingToken).ConfigureAwait(false);

            get.Description = "prisma demo";

            PrismaDemo update = await provider.UpdateAsync(get, stoppingToken).ConfigureAwait(false);

            bool delete = await provider.DeleteAsync(get.Id, stoppingToken).ConfigureAwait(false);

        }

        #endregion
    }
}
