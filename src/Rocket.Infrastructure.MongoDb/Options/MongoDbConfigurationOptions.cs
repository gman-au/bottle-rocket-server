namespace Rocket.Infrastructure.MongoDb.Options
{
    public class MongoDbConfigurationOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}