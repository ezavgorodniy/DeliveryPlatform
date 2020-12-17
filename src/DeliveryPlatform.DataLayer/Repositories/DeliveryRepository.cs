using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryPlatform.DataLayer.DataModels;
using DeliveryPlatform.DataLayer.Interfaces;
using DeliveryPlatform.DataLayer.Mongo.Interfaces;
using MongoDB.Driver;

namespace DeliveryPlatform.DataLayer.Repositories
{
    internal class DeliveryRepository : IDeliveryRepository
    {
        private const string CollectionName = "deliveries";
        private readonly IMongoDatabaseFactory _mongoDbFactory;


        public DeliveryRepository(IMongoDatabaseFactory mongoDbFactory)
        {
            _mongoDbFactory = mongoDbFactory;
        }

        public async Task<Delivery> Create(Delivery entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Id = Guid.NewGuid().ToString();
            entity.State = DeliveryState.Created;

            var collection = GetCollection();
            await collection.InsertOneAsync(entity);

            return await Get(entity.Id);
        }

        public async Task<IEnumerable<Delivery>> GetAll()
        {
            var collection = GetCollection();
            var all = await collection.Find(x => true).ToListAsync();
            return all;
        }

        public async Task<Delivery> Get(string id)
        {
            var collection = GetCollection();
            var delivery = await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            return delivery;
        }

        public async Task<bool> Delete(string id)
        {
            var collection = GetCollection();
            var result = await collection.DeleteOneAsync(Builders<Delivery>.Filter.Eq(x => x.Id, id));
            return result.DeletedCount != 0;
        }

        public async Task<Delivery> Update(Delivery entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var collection = GetCollection();
            await collection.UpdateOneAsync(Builders<Delivery>.Filter.Eq(x => x.Id, entity.Id),
                Builders<Delivery>.Update.Set(x => x.State, entity.State));

            return await Get(entity.Id);
        }

        public async Task MarkExpiredDeliveries()
        {
            var collection = GetCollection();
            await collection.UpdateManyAsync(
                Builders<Delivery>.Filter.And(
                    Builders<Delivery>.Filter.Or(
                        Builders<Delivery>.Filter.Eq(x => x.State, DeliveryState.Created),
                        Builders<Delivery>.Filter.Eq(x => x.State, DeliveryState.Approved)),
                    Builders<Delivery>.Filter.Lt(x => x.AccessWindow.EndTime, DateTime.Now)),
                Builders<Delivery>.Update.Set(x => x.State, DeliveryState.Expired));
        }

        private IMongoCollection<Delivery> GetCollection()
        {
            var mongoDb = _mongoDbFactory.Connect();
            return mongoDb.GetCollection<Delivery>(CollectionName);
        }
    }
}
