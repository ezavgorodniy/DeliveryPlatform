using MongoDB.Driver;

namespace DeliveryPlatform.DataLayer.Mongo.Interfaces
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase Connect();
    }
}
