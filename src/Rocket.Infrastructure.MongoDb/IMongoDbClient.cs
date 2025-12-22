using MongoDB.Driver;

namespace Rocket.Infrastructure.MongoDb
{
    public interface IMongoDbClient
    {
        IMongoClient GetClient();
        
        IMongoDatabase GetDatabase();
    }
}