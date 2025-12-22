using MongoDB.Driver;

namespace Rocket.Infrastructure.Db.Mongo
{
    public interface IMongoDbClient
    {
        IMongoClient GetClient();
        
        IMongoDatabase GetDatabase();
    }
}