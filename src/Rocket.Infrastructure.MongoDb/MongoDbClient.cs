using System.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Rocket.Infrastructure.MongoDb.Options;

namespace Rocket.Infrastructure.MongoDb
{
    public class MongoDbClient : IMongoDbClient
    {
        private readonly IMongoClient _mongoClient;

        public MongoDbClient(
            IOptions<MongoDbConfigurationOptions> optionsAccessor,
            ILogger<MongoDbClient> logger
        )
        {
            try
            {
                logger
                    .LogInformation("Connecting to MongoDB");

                var options = optionsAccessor.Value;

                _mongoClient =
                    new MongoClient(options?.ConnectionString ?? throw new ConfigurationErrorsException(nameof(options.ConnectionString)));

                logger
                    .LogInformation("Successfully connected to MongoDB");
            }
            catch (MongoConfigurationException ex)
            {
                logger
                    .LogCritical(
                        "Could not connect to MongoDB: {error}",
                        ex.Message
                    );

                throw new ConfigurationErrorsException(
                    "Could not connect to MongoDB.",
                    ex
                );
            }
        }

        public IMongoClient GetClient() => _mongoClient;
    }
}