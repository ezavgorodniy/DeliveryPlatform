using DeliveryPlatform.DataLayer.Interfaces;
using DeliveryPlatform.DataLayer.Models;
using DeliveryPlatform.DataLayer.Mongo.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DeliveryPlatform.DataLayer.Mongo
{
    internal class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        private const string DbName = "default";
        private readonly ConnectionStringSettings _connectionStringSettings;

        public MongoDatabaseFactory(IOptions<ConnectionStringSettings> settings)
        {
            _connectionStringSettings = settings.Value;

        }

        public IMongoDatabase Connect()
        {
            var mongoClient = new MongoClient(_connectionStringSettings.DbConnectionString);
            return mongoClient.GetDatabase(DbName);
        }
    }
}
