using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Rocket.Domain;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbGlobalSettingsRepository(
        ILogger<MongoDbGlobalSettingsRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<BaseConnector>(
        mongoDbClient,
        logger
    ), IGlobalSettingsRepository
    {
        protected override string CollectionName => MongoConstants.GlobalSettingsCollection;

        private static readonly BsonDocument Defaults =
            new()
            {
                { nameof(GlobalSettings.SweepSuccessfulScansAfterDays), 10 },
                { nameof(GlobalSettings.EnableSweeping), true }
            };

        public async Task UpdateGlobalSettingsAsync(CancellationToken cancellationToken)
        {
            var filter =
                Builders<BsonDocument>
                    .Filter
                    .Empty;

            var pipelineStages =
                new List<BsonDocument>
                {
                    new(
                        "$set",
                        BuildSetIfMissingPipeline()
                    )
                };

            var pipeline =
                PipelineDefinition<BsonDocument, BsonDocument>
                    .Create(pipelineStages);

            var collection =
                GetMongoCollection<BsonDocument>();

            await
                collection
                    .UpdateOneAsync(
                        filter,
                        pipeline,
                        new UpdateOptions
                        {
                            IsUpsert = true
                        },
                        cancellationToken
                    );
        }

        public async Task<GlobalSettings> GetGlobalSettingsAsync(CancellationToken cancellationToken)
        {
            var collection =
                GetMongoCollection<GlobalSettings>();

            var settings =
                await
                    collection
                        .Find(Builders<GlobalSettings>.Filter.Empty)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return settings;
        }

        private static BsonDocument BuildSetIfMissingPipeline()
        {
            var fields = new BsonDocument();

            foreach (var field in Defaults)
            {
                fields[field.Name] =
                    new BsonDocument(
                        "$ifNull",
                        new BsonArray
                        {
                            "$" + field.Name,
                            field.Value
                        });
            }

            return fields;
        }
    }
}