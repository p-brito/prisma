using FluentAssertions;

using Prisma.Core;
using Prisma.Core.Entities;
using Prisma.Core.Exceptions;
using Prisma.Core.Extensions;
using Prisma.Core.Providers;
using Prisma.UnitTests.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Microsoft.Azure.Cosmos;
using Moq;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace Prisma.Providers.UnitTests
{
    public class CosmosDbProviderTests
    {
        #region Test Methods

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public void Should_Get_Provider()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            Assert.NotNull(provider);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Get_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            TestEntity entity = await provider.GetAsync("myEntity").ConfigureAwait(false);

            entity.Should().NotBeNull();

            entity.Id.Should().Be("myEntity");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Get_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            TestEntity entity = await provider.GetAsync("dontExist").ConfigureAwait(false);

            entity.Should().BeNull();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowException_On_Get_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            Func<Task> action = () => provider.GetAsync(null);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Insert_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            TestEntity entity = await provider.InsertAsync(GetTestEntity()).ConfigureAwait(false);

            entity.Should().NotBeNull();

            entity.Name.Should().Be("Test entity");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Insert_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            Func<Task> action = () => provider.InsertAsync(GetTestEntity());

            // Assert

            await action.Should().ThrowAsync<PrismaException>().ConfigureAwait(false);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowException_On_Insert_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            Func<Task> action = () => provider.InsertAsync(null);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }


        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Update_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            TestEntity entity = await provider.UpdateAsync(GetTestEntity()).ConfigureAwait(false);

            entity.Should().NotBeNull();

            entity.Name.Should().Be("Test entity");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Update_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            Func<Task> action = () => provider.UpdateAsync(GetTestEntity());

            // Assert

            await action.Should().ThrowAsync<PrismaException>().ConfigureAwait(false);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowExcepion_On_Update_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            Func<Task> action = () => provider.UpdateAsync(null);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }


        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Delete_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            bool deleted = await provider.DeleteAsync("myEntity123").ConfigureAwait(false);

            deleted.Should().BeTrue();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Delete_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            bool deleted = await provider.DeleteAsync("myEntity123").ConfigureAwait(false);

            deleted.Should().BeFalse();
        }


        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowException_On_Delete_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IPrismaProvider<TestEntity> provider = serviceProvider.GetRequiredService<IPrismaProvider<TestEntity>>();

            Func<Task> action = () => provider.DeleteAsync(null);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        #endregion

        #region Private Methods

        private static IServiceProvider BuildServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Add options

            services
                    .AddOptions()
                    .AddOptionsSnapshot<PrismaOptions>();

            services.AddLogging();

            Dictionary<string, string> settings = new()
            {
                { "PrismaOptions:Provider", "CosmosDb" },
                { "PrismaOptions:ConnectionString", "AccountEndpoint=test;AccountKey=test;DatabaseKey=test;AutoscaleThroughput=3000" },
            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            services.Configure<PrismaOptions>(configuration.GetSection(nameof(PrismaOptions)));

            // Add Cosmos mock logic

            AddCosmosMocks(services);


            // Add provider

            services.TryAddTransient(typeof(IPrismaProvider<>), typeof(CosmosDbProvider<>));

            // Build

            return services.BuildServiceProvider();

        }

        private static IServiceProvider BuildBrokenServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Add options

            services
                    .AddOptions()
                    .AddOptionsSnapshot<PrismaOptions>();

            services.AddLogging();

            Dictionary<string, string> settings = new()
            {
                { "PrismaOptions:Provider", "CosmosDb" },
                { "PrismaOptions:ConnectionString", "AccountEndpoint=test;AccountKey=test;DatabaseKey=test;AutoscaleThroughput=3000" },
            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            services.Configure<PrismaOptions>(configuration.GetSection(nameof(PrismaOptions)));

            // Add Mongo mock logic

            AddCosmosBrokenMocks(services);


            // Add provider

            services.TryAddTransient(typeof(IPrismaProvider<>), typeof(CosmosDbProvider<>));

            // Build

            return services.BuildServiceProvider();

        }

        private static IServiceCollection AddCosmosMocks(IServiceCollection services)
        {
            // Create mocks

            Mock<CosmosClient> client = new();

            Mock<Database> database = new();

            Mock<Container> container = new();

            Mock<DatabaseResponse> dbResponse = new();

            Mock<ContainerResponse> ctResponse = new();

            Mock<ItemResponse<TestEntity>> itemResponse = new();

            Mock<FeedIterator<TestEntity>> queryIterator = new();

            Mock<FeedResponse<TestEntity>> feedResponse = new();

            // Setup database response

            dbResponse.Setup(_ => _.StatusCode).Returns(System.Net.HttpStatusCode.Created);

            dbResponse.Setup(_ => _.Database).Returns(database.Object);

            // Setup container response

            ctResponse.Setup(_ => _.StatusCode).Returns(System.Net.HttpStatusCode.Created);

            ctResponse.Setup(_ => _.Container).Returns(container.Object);

            // Setup item response

            itemResponse.Setup(_ => _.StatusCode).Returns(System.Net.HttpStatusCode.OK);

            itemResponse.Setup(_ => _.Resource).Returns( GetTestEntity() );

            // Setup feed response

            feedResponse.Setup(_ => _.Resource).Returns(new List<TestEntity> { GetTestEntity() });

            // Setup query iterator

            queryIterator.Setup(_ => _.HasMoreResults).Returns(true);

            queryIterator.Setup(_ => _.ReadNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(feedResponse.Object);

            // Setup container

            container.Setup(_ => _.DeleteItemAsync<TestEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(itemResponse.Object);

            container.Setup(_ => _.GetItemQueryIterator<TestEntity>(It.IsAny<string>(), null, null)).Returns(queryIterator.Object);

            container.Setup(_ => _.CreateItemAsync<TestEntity>(It.IsAny<TestEntity>(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(itemResponse.Object);

            container.Setup(_ => _.UpsertItemAsync<TestEntity>(It.IsAny<TestEntity>(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(itemResponse.Object);

            // Setup database

            database.Setup(_ => _.GetContainer(It.IsAny<string>())).Returns(container.Object);

            database.Setup(_ => _.CreateContainerIfNotExistsAsync(It.IsAny<ContainerProperties>(), It.IsAny<ThroughputProperties>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(ctResponse.Object);

            // Setup client

            client.Setup(_ => _.CreateDatabaseIfNotExistsAsync(It.IsAny<string>(), It.IsAny<ThroughputProperties>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(dbResponse.Object);

            client.Setup(_ => _.GetDatabase(It.IsAny<string>())).Returns(database.Object);


            services.AddSingleton(client.Object);

            return services;
        }

        private static IServiceCollection AddCosmosBrokenMocks(IServiceCollection services)
        {
            // Create mocks

            Mock<CosmosClient> client = new();

            Mock<Database> database = new();

            Mock<Container> container = new();

            Mock<DatabaseResponse> dbResponse = new();

            Mock<ContainerResponse> ctResponse = new();

            Mock<ItemResponse<TestEntity>> itemResponse = new();

            Mock<FeedIterator<TestEntity>> queryIterator = new();

            Mock<FeedResponse<TestEntity>> feedResponse = new();

            // Setup database response

            dbResponse.Setup(_ => _.StatusCode).Returns(System.Net.HttpStatusCode.Created);

            dbResponse.Setup(_ => _.Database).Returns(database.Object);

            // Setup container response

            ctResponse.Setup(_ => _.StatusCode).Returns(System.Net.HttpStatusCode.Created);

            ctResponse.Setup(_ => _.Container).Returns(container.Object);

            // Setup item response

            itemResponse.Setup(_ => _.StatusCode).Returns(System.Net.HttpStatusCode.InternalServerError);

            itemResponse.Setup(_ => _.Resource).Returns(GetTestEntity());

            // Setup feed response

            feedResponse.Setup(_ => _.Resource).Returns(new List<TestEntity>());

            // Setup query iterator

            queryIterator.Setup(_ => _.HasMoreResults).Returns(false);

            // Setup container

            container.Setup(_ => _.DeleteItemAsync<TestEntity>(It.IsAny<string>(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(itemResponse.Object);

            container.Setup(_ => _.GetItemQueryIterator<TestEntity>(It.IsAny<string>(), null, null)).Returns(queryIterator.Object);

            container.Setup(_ => _.CreateItemAsync<TestEntity>(It.IsAny<TestEntity>(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(itemResponse.Object);

            container.Setup(_ => _.UpsertItemAsync<TestEntity>(It.IsAny<TestEntity>(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(itemResponse.Object);

            // Setup database

            database.Setup(_ => _.GetContainer(It.IsAny<string>())).Returns(container.Object);

            database.Setup(_ => _.CreateContainerIfNotExistsAsync(It.IsAny<ContainerProperties>(), It.IsAny<ThroughputProperties>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(ctResponse.Object);

            // Setup client

            client.Setup(_ => _.CreateDatabaseIfNotExistsAsync(It.IsAny<string>(), It.IsAny<ThroughputProperties>(), null, It.IsAny<CancellationToken>())).ReturnsAsync(dbResponse.Object);

            client.Setup(_ => _.GetDatabase(It.IsAny<string>())).Returns(database.Object);


            services.AddSingleton(client.Object);

            return services;
        }

        private static TestEntity GetTestEntity()
        {
            return new TestEntity()
            {
                Id = "myEntity",
                CreatedOn = DateTime.UtcNow,
                Name = "Test entity",
            };
        }

        #endregion
    }
}