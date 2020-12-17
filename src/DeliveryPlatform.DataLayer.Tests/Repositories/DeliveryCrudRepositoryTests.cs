using System;
using System.Threading.Tasks;
using DeliveryPlatform.DataLayer.DataModels;
using DeliveryPlatform.DataLayer.Mongo.Interfaces;
using DeliveryPlatform.DataLayer.Repositories;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace DeliveryPlatform.DataLayer.Tests.Repositories
{
    public class  DeliveryCrudRepositoryTests : IDisposable
    {
        private const string TestDatabase = "testDatabase";
        private readonly MongoDbRunner _runner;
        private readonly DeliveryRepository _repo;

        public DeliveryCrudRepositoryTests()
        {
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);

            foreach (var database in client.ListDatabaseNames().ToList())
            {
                if (database == TestDatabase)
                {
                    client.DropDatabase(TestDatabase);
                }
            }
            var mongoDatabase = client.GetDatabase(TestDatabase);


            var mockFactory = new Mock<IMongoDatabaseFactory>();
            mockFactory.Setup(factory => factory.Connect()).Returns(mongoDatabase);
            _repo = new DeliveryRepository(mockFactory.Object);
        }

        [Fact]
        public async Task CreateNullExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repo.Create(null));
        }

        [Fact]
        public async Task CreateExpectAddEntityToRepo()
        {
            var prevAll = await _repo.GetAll();
            Assert.Empty(prevAll);

            const string givenId = "givenId";
            var entity = new Delivery
            {
                Id = givenId
            };

            var created = await _repo.Create(entity);
            Assert.Single(await _repo.GetAll());

            Assert.NotEqual(givenId, created.Id);
        }

        [Fact]
        public async Task CreateExpectGeneratedId()
        {
            var prevAll = await _repo.GetAll();
            Assert.Empty(prevAll);

            const string givenId = "givenId";
            var entity = new Delivery
            {
                Id = givenId
            };

            var created = await _repo.Create(entity);
            Assert.Single(await _repo.GetAll());

            var foundByGivenId = await _repo.Get(givenId);
            Assert.Null(foundByGivenId);

            var foundByGeneratedId = await _repo.Get(created.Id);
            Assert.NotNull(foundByGeneratedId);
        }

        [Fact]
        public async Task DeleteExpectToDelete()
        {
            var prevAll = await _repo.GetAll();
            Assert.Empty(prevAll);

            const string givenId = "givenId";
            var entity = new Delivery
            {
                Id = givenId
            };

            var created = await _repo.Create(entity);

            var deleteByGivenId = await _repo.Delete(givenId);
            Assert.False(deleteByGivenId);
            Assert.Single(await _repo.GetAll());

            var deleteByGeneratedId = await _repo.Delete(created.Id);
            Assert.True(deleteByGeneratedId);
            Assert.Empty(await _repo.GetAll());
        }

        [Fact]
        public async Task UpdateNullExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repo.Update(null));
        }

        [Fact]
        public async Task UpdateNotFoundEntityExpectNull()
        {
            const string id = "id";
            var entity = new Delivery
            {
                Id = id
            };

            var result = await _repo.Update(entity);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateExpectUpdateState()
        {
            const string id = "id";
            const DeliveryState prevState = DeliveryState.Approved;
            var entity = new Delivery
            {
                Id = id,
                State = prevState
            };

            var createdEntity = await _repo.Create(entity);

            const DeliveryState newState = DeliveryState.Completed;
            var updatedEntity = new Delivery
            {
                Id = createdEntity.Id,
                State = newState
            };

            var updateEntity = await _repo.Update(updatedEntity);

            Assert.Equal(newState, updateEntity.State);

            var foundEntity = await _repo.Get(createdEntity.Id);

            Assert.Equal(newState, foundEntity.State);
        }

        [Theory]
        [InlineData(DeliveryState.Approved, DeliveryState.Expired)]
        [InlineData(DeliveryState.Created, DeliveryState.Expired)]
        [InlineData(DeliveryState.Expired, DeliveryState.Expired)]
        [InlineData(DeliveryState.Completed, DeliveryState.Completed)]
        [InlineData(DeliveryState.Cancelled, DeliveryState.Cancelled)]
        public async Task MarkDeliveryAsCompletedTest(DeliveryState oldState, DeliveryState newState)
        {
            var expiredDelivery = new Delivery
            {
                AccessWindow = new AccessWindow
                {
                    EndTime = DateTime.Now.AddHours(-10)
                },
                State = oldState
            };

            var createdDelivery = await _repo.Create(expiredDelivery);

            var newDelivery = new Delivery
            {
                State = oldState,
                Id = createdDelivery.Id
            };

            await _repo.Update(newDelivery);

            await _repo.MarkExpiredDeliveries();

            var delivery = await _repo.Get(createdDelivery.Id);

            Assert.Equal(newState, delivery.State);
        }
        

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
