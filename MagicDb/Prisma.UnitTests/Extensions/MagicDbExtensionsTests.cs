using FluentAssertions;

using Prisma.Core;
using Prisma.Core.DependencyInjection;
using Prisma.Core.Entities;
using Prisma.UnitTests.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace Prisma.UnitTests.Extensions
{
    public sealed class PrismaExtensionsTests
    {
        #region Test Methods

        /// <summary>
        /// Should add Prisma to the service provider.
        /// </summary>
        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public void Should_Add_Prisma_To_ServiceProvider()
        {
            // Create services

            IServiceProvider provider = BuildProviderWithOptions();

            // Get Db providers

            IPrismaProvider<TestEntity> testProvider = provider.GetRequiredService<IPrismaProvider<TestEntity>>();

            // Assert

            Assert.NotNull(testProvider);
        }

        /// <summary>
        /// Should add Prisma to the service provider.
        /// </summary>
        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public void Should_Not_Add_Prisma_To_ServiceProvider()
        {
            // Create services

            IServiceProvider provider = BuildProviderWithoutOptions();

            // Get Db providers

            Func<IPrismaProvider<TestEntity>> action = () => provider.GetRequiredService<IPrismaProvider<TestEntity>>();

            // Assert

            action.Should().Throw<InvalidOperationException>();
        }

        #endregion

        #region Private Methods

        private static IServiceProvider BuildProviderWithOptions()
        {
            IServiceCollection services = new ServiceCollection();

            //Arrange

            Dictionary<string, string> settings = new()
            {
                { "PrismaOptions:Provider", "MongoDb"},
                { "PrismaOptions:ConnectionString", "mongodb+srv://magicboxadmin:Fe!Ru!eQZa!TNg6@magicboxcluster.c900q.mongodb.net/Prisma?retryWrites=true&w=majority" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            services.Configure<PrismaOptions>(configuration.GetSection(nameof(PrismaOptions)));

            services.AddPrisma();

            return services.BuildServiceProvider();
        }

        private static IServiceProvider BuildProviderWithoutOptions()
        {
            IServiceCollection services = new ServiceCollection();

            //Arrange

            services.AddPrisma();

            return services.BuildServiceProvider();
        }

        #endregion
    }
}
