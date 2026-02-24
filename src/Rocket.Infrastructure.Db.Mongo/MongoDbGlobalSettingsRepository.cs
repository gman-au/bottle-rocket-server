using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbGlobalSettingsRepository(
        ILogger<MongoDbGlobalSettingsRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<GlobalSettings>(
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

        public async Task UpdateGlobalSettingsAsync(
            int? sweepSuccessfulScansAfterDays,
            bool? enableSweeping,
            CancellationToken cancellationToken
        )
        {
            var filter =
                Builders<GlobalSettings>
                    .Filter
                    .Empty;

            var updateBuilder = Builders<GlobalSettings>.Update;
            var updates = new List<UpdateDefinition<GlobalSettings>>();

            if (enableSweeping.HasValue)
            {
                updates
                    .Add(
                        updateBuilder
                            .Set(
                                o => o.EnableSweeping,
                                enableSweeping.Value
                            )
                    );
            }

            if (sweepSuccessfulScansAfterDays.HasValue)
            {
                updates
                    .Add(
                        updateBuilder
                            .Set(
                                o => o.SweepSuccessfulScansAfterDays,
                                sweepSuccessfulScansAfterDays.Value
                            )
                    );
            }

            if (updates.Count == 0) return;

            var combinedUpdate = updateBuilder.Combine(updates);

            var collection =
                GetMongoCollection();

            await
                collection
                    .UpdateOneAsync(
                        filter,
                        combinedUpdate,
                        new UpdateOptions(),
                        cancellationToken
                    );
        }

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
                        }
                    );
            }

            return fields;
        }
    }
}