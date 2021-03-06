using FluentAssertions;
using MagicDb.Core;
using MagicDb.Core.Entities;
using MagicDb.Core.Exceptions;
using MagicDb.Core.Extensions;
using MagicDb.Core.Providers;
using MagicDb.UnitTests.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using Moq;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MagicDb.Providers.UnitTests
{
    public class MongoDbProviderTests
    {
        #region Test Methods

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public void Should_Get_Provider()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            Assert.NotNull(mongoDbProvider);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Get_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            TestEntity entity = await mongoDbProvider.GetAsync("myEntity123").ConfigureAwait(false);

            entity.Should().NotBeNull();

            entity.Id.Should().Be("myEntity123");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Get_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            TestEntity entity = await mongoDbProvider.GetAsync("dontExist").ConfigureAwait(false);

            entity.Should().BeNull();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowException_On_Get_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            Func<Task> action = () => mongoDbProvider.GetAsync(null);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Insert_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            TestEntity testEntity = new()
            {
                Name = "InsertedEntity"
            };

            TestEntity entity = await mongoDbProvider.InsertAsync(testEntity).ConfigureAwait(false);

            entity.Should().NotBeNull();

            entity.Name.Should().Be("InsertedEntity");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Insert_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            TestEntity testEntity = new()
            {
                Name = "InsertedEntity"
            };

            Func<Task> action = () => mongoDbProvider.InsertAsync(testEntity);

            // Assert

            await action.Should().ThrowAsync<MagicDbException>().ConfigureAwait(false);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowException_On_Insert_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            Func<Task> action = () => mongoDbProvider.InsertAsync(null);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }


        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Update_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            TestEntity testEntity = new()
            {
                Id = "myEntity123",
                CreatedOn = DateTime.UtcNow,
                Name = "Test entity",
            };

            TestEntity entity = await mongoDbProvider.UpdateAsync(testEntity).ConfigureAwait(false);

            entity.Should().NotBeNull();

            entity.Name.Should().Be("Test entity");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Update_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            TestEntity testEntity = new()
            {
                Id = "myEntity123",
                CreatedOn = DateTime.UtcNow,
                Name = "Test entity",
            };

            Func<Task> action = () => mongoDbProvider.UpdateAsync(testEntity);

            // Assert

            await action.Should().ThrowAsync<MagicDbException>().ConfigureAwait(false);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowExcepion_On_Update_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            Func<Task> action = () => mongoDbProvider.UpdateAsync(null);

            // Assert

            await action.Should().ThrowAsync<ArgumentNullException>().ConfigureAwait(false);
        }


        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Delete_Entity()
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            bool deleted = await mongoDbProvider.DeleteAsync("myEntity123").ConfigureAwait(false);

            deleted.Should().BeTrue();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_Not_Delete_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            bool deleted = await mongoDbProvider.DeleteAsync("myEntity123").ConfigureAwait(false);

            deleted.Should().BeFalse();
        }


        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
        public async Task Should_ThrowException_On_Delete_Entity()
        {
            IServiceProvider serviceProvider = BuildBrokenServiceProvider();

            IMagicDbProvider<TestEntity> mongoDbProvider = serviceProvider.GetRequiredService<IMagicDbProvider<TestEntity>>();

            Func<Task> action = () => mongoDbProvider.DeleteAsync(null);

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
                    .AddOptionsSnapshot<MagicDbOptions>();

            Dictionary<string, string> settings = new()
            {
                { "MagicDbOptions:Provider", "MongoDb" },
                { "MagicDbOptions:ConnectionString", "mongodb+srv://admin:mypass123@myddbcluster.c900q.mongodb.net/magicboxDb?retryWrites=true&w=majority" },
                { "MagicDbOptions:DatabaseName", "Default" }
            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            services.Configure<MagicDbOptions>(configuration.GetSection(nameof(MagicDbOptions)));

            // Add Mongo mock logic

            AddMongoMocks(services);


            // Add provider

            services.TryAddTransient(typeof(IMagicDbProvider<>), typeof(MongoDbProvider<>));

            // Build

            return services.BuildServiceProvider();

        }

        private static IServiceProvider BuildBrokenServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Add options

            services
                    .AddOptions()
                    .AddOptionsSnapshot<MagicDbOptions>();

            Dictionary<string, string> settings = new()
            {
                { "MagicDbOptions:Provider", "MongoDb" },
                { "MagicDbOptions:ConnectionString", "mongodb+srv://admin:mypass123@myddbcluster.c900q.mongodb.net/magicboxDb?retryWrites=true&w=majority" },
                { "MagicDbOptions:DatabaseName", "Default" }
            };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            services.Configure<MagicDbOptions>(configuration.GetSection(nameof(MagicDbOptions)));

            // Add Mongo mock logic

            AddMongoBrokenMocks(services);


            // Add provider

            services.TryAddTransient(typeof(IMagicDbProvider<>), typeof(MongoDbProvider<>));

            // Build

            return services.BuildServiceProvider();

        }


        private static IServiceCollection AddMongoMocks(IServiceCollection services)
        {
            // Create Mocks

            TestEntity test = new()
            {
                Id = "myEntity123",
                CreatedOn = DateTime.UtcNow,
                Name = "Test entity",
            };

            IList<string> expectedCollection = new List<string>()
            {
                "TestEntitys"
            };

            IList<TestEntity> expectedEntities = new List<TestEntity>()
            {
                test
            };

            Mock<IMongoClient> client = new();

            Mock<IMongoDatabase> mongoDb = new();

            Mock<IMongoCollection<TestEntity>> collection = new();

            Mock<IAsyncCursor<string>> mockCursor = new();

            Mock<IClientSessionHandle> mockClientSessionHandle = new();

            Mock<IAsyncCursor<TestEntity>> mockEntityCursor = new();

            Mock<DeleteResult> mockDeleteResult = new();

            // Setup mock delete result

            mockDeleteResult.Setup(d =>d.IsAcknowledged).Returns(true);
            mockDeleteResult.Setup(d => d.DeletedCount).Returns(1);

            // Setup cursors

            mockEntityCursor.Setup(_ => _.Current).Returns(expectedEntities);
            mockEntityCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            mockCursor.Setup(_ => _.Current).Returns(expectedCollection);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            // Setup collection

            collection.Setup(
                c => c.FindAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(), 
                    It.IsAny<FindOptions<TestEntity, TestEntity>>(), 
                    It.IsAny<CancellationToken>())).ReturnsAsync(mockEntityCursor.Object);

            collection.Object.InsertOne(test);

            collection.Setup(
                c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(), 
                    It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockDeleteResult.Object));

            // Setup db

            mongoDb.Setup(
                db => db.ListCollectionNames(
                    It.IsAny<ListCollectionNamesOptions>(), 
                    It.IsAny<CancellationToken>())).Returns(mockCursor.Object);

            mongoDb.Setup(
                db => db.GetCollection<TestEntity>(
                    It.IsAny<string>(), 
                    It.IsAny<MongoCollectionSettings>())).Returns(collection.Object);

            // Setup client

            client.Setup(
                c => c.GetDatabase(
                    It.IsAny<string>(),
                    It.IsAny<MongoDatabaseSettings>())).Returns(mongoDb.Object);

            client.Setup(
                c => c.StartSession(
                    It.IsAny<ClientSessionOptions>(), 
                    It.IsAny<CancellationToken>())).Returns(mockClientSessionHandle.Object);

            services.TryAddSingleton(client.Object);

            return services;
        }

        private static IServiceCollection AddMongoBrokenMocks(IServiceCollection services)
        {
            // Create Mocks

            IList<string> expectedCollection = new List<string>()
            {
                "TestEntitys"
            };

            IList<TestEntity> expectedEntities = new List<TestEntity>();

            Mock<IMongoClient> client = new();

            Mock<IMongoDatabase> mongoDb = new();

            Mock<IMongoCollection<TestEntity>> collection = new();

            Mock<IAsyncCursor<string>> mockCursor = new();

            Mock<IClientSessionHandle> mockClientSessionHandle = new();

            Mock<IAsyncCursor<TestEntity>> mockEntityCursor = new();

            Mock<DeleteResult> mockDeleteResult = new();
            // Setup mock delete result

            mockDeleteResult.Setup(d => d.IsAcknowledged).Returns(false);
            mockDeleteResult.Setup(d => d.DeletedCount).Returns(0);

            // Setup cursors

            mockEntityCursor.Setup(_ => _.Current).Returns(expectedEntities);
            mockEntityCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            mockCursor.Setup(_ => _.Current).Returns(expectedCollection);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true))
                .Returns(Task.FromResult(false));

            // Setup collection

            collection.Setup(
                c => c.FindAsync(It.IsAny<FilterDefinition<TestEntity>>(), 
                                 It.IsAny<FindOptions<TestEntity, TestEntity>>(), 
                                 It.IsAny<CancellationToken>())).ReturnsAsync(mockEntityCursor.Object);

            collection.Setup(
                c => c.InsertOneAsync(
                                It.IsAny<IClientSessionHandle>(), 
                                It.IsAny<TestEntity>(), 
                                It.IsAny<InsertOneOptions>(), 
                                It.IsAny<CancellationToken>())).ThrowsAsync(new MongoException("Test error"));

            collection.Setup(
                c => c.ReplaceOneAsync(
                                It.IsAny<IClientSessionHandle>(), 
                                It.IsAny<FilterDefinition<TestEntity>>(), 
                                It.IsAny<TestEntity>(), 
                                It.IsAny<ReplaceOptions>(), 
                                It.IsAny<CancellationToken>())).ThrowsAsync(new MongoException("Test error"));

            collection.Setup(
                c => c.DeleteOneAsync(
                                It.IsAny<FilterDefinition<TestEntity>>(), 
                                It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockDeleteResult.Object));

            // Setup db

            mongoDb.Setup(
                db => db.ListCollectionNames(
                    It.IsAny<ListCollectionNamesOptions>(), 
                    It.IsAny<CancellationToken>())).Returns(mockCursor.Object);

            mongoDb.Setup(
                db => db.GetCollection<TestEntity>(
                    It.IsAny<string>(), 
                    It.IsAny<MongoCollectionSettings>())).Returns(collection.Object);

            // Setup client

            client.Setup(
                c => c.GetDatabase(
                    It.IsAny<string>(), 
                    It.IsAny<MongoDatabaseSettings>())).Returns(mongoDb.Object);

            client.Setup(
                c => c.StartSession(
                    It.IsAny<ClientSessionOptions>(),
                    It.IsAny<CancellationToken>())).Returns(mockClientSessionHandle.Object);

            services.TryAddSingleton(client.Object);

            return services;
        }

        #endregion
    }
}