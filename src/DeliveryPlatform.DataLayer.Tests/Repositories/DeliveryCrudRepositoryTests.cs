using System;
using System.Threading.Tasks;
using DeliveryPlatform.DataLayer.DataModels;
using DeliveryPlatform.DataLayer.Repositories;
using Xunit;

namespace DeliveryPlatform.DataLayer.Tests.Repositories
{
    public class  DeliveryCrudRepositoryTests
    {
        private readonly DeliveryCrudRepository _repo;

        public DeliveryCrudRepositoryTests()
        {
            _repo = new DeliveryCrudRepository();
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
    }
}
