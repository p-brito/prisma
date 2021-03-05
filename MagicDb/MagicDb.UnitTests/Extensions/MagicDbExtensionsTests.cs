using FluentAssertions;
using MagicDb.Core;
using MagicDb.Core.DependencyInjection;
using MagicDb.Core.Entities;
using MagicDb.UnitTests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MagicDb.UnitTests.Extensions
{
    public sealed class MagicDbExtensionsTests
    {
        #region Test Methods

        /// <summary>
        /// Should add MagicDb to the service provider.
        /// </summary>
        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public void Should_Add_MagicDb_To_ServiceProvider()
        {
            // Create services

            IServiceProvider provider = BuildProviderWithOptions();

            // Get Db providers

            IMagicDbProvider<TestEntity> testProvider = provider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            // Assert

            Assert.NotNull(testProvider);
        }

        /// <summary>
        /// Should add MagicDb to the service provider.
        /// </summary>
        [Fact]
        [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
        public void Should_Not_Add_MagicDb_To_ServiceProvider()
        {
            // Create services

            IServiceProvider provider = BuildProviderWithoutOptions();

            // Get Db providers

            Func<IMagicDbProvider<TestEntity>> action = () => provider.GetRequiredService<IMagicDbProvider<TestEntity>>();

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
                {"MagicDbOptions:Provider", "MongoDb"},
                {"MagicDbOptions:ConnectionString", "mongodb+srv://magicboxadmin:Fe!Ru!eQZa!TNg6@magicboxdbcluster.c900q.mongodb.net/magicboxDb?retryWrites=true&w=majority"},
                { "MagicDbOptions:DatabaseName", "Default"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            services.Configure<MagicDbOptions>(configuration.GetSection(nameof(MagicDbOptions)));

            services.AddMagicDb();

            return services.BuildServiceProvider();
        }

        private static IServiceProvider BuildProviderWithoutOptions()
        {
            IServiceCollection services = new ServiceCollection();

            //Arrange

            services.AddMagicDb();

            return services.BuildServiceProvider();
        }

        #endregion
    }
}
