using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.PageTemplates;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbRocketbookPageTemplateRepository(
        ILogger<MongoDbRocketbookPageTemplateRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<RocketbookPageTemplate>(
        mongoDbClient,
        logger
    ), IRocketbookPageTemplateRepository
    {
        protected override string CollectionName => MongoConstants.RocketbookPageTemplateCollection;

        public async Task<long> UpsertPageTemplateAsync(
            RocketbookPageTemplate rocketbookPageTemplate,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var filter =
                    Builders<RocketbookPageTemplate>
                        .Filter
                        .Eq(
                            o => o.QrCode,
                            rocketbookPageTemplate.QrCode
                        );

                var update =
                    Builders<RocketbookPageTemplate>
                        .Update
                        .Set(
                            o => o.PaperSizeType,
                            rocketbookPageTemplate.PaperSizeType
                        )
                        .Set(
                            o => o.QrCodeOrientationType,
                            rocketbookPageTemplate.QrCodeOrientationType
                        )
                        .Set(
                            o => o.RocketbookPageTemplateType,
                            rocketbookPageTemplate.RocketbookPageTemplateType
                        )
                        .Set(
                            o => o.SymbolsBoundingBox,
                            rocketbookPageTemplate.SymbolsBoundingBox
                        );

                var result =
                    await
                        GetMongoCollection()
                            .UpdateOneAsync(
                                filter,
                                update,
                                new UpdateOptions { IsUpsert = true },
                                cancellationToken
                            );

                return
                    result.UpsertedId != null ? 1 : 0;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error saving the ({type}) record: {error}",
                        nameof(RocketbookPageTemplate),
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<IEnumerable<RocketbookPageTemplate>> FetchAllAsync(CancellationToken cancellationToken)
        {
            var filter =
                Builders<RocketbookPageTemplate>
                    .Filter
                    .Empty;

            var collection =
                GetMongoCollection();

            var allRecords =
                await
                    collection
                        .Find(filter)
                        .ToListAsync(cancellationToken);

            return allRecords;
        }

        public async Task<RocketbookPageTemplate> GetTemplateByQrCodeAsync(string qrCode, CancellationToken cancellationToken)
            =>
                await
                    FetchFirstFilteredRecordAsync(
                        Builders<RocketbookPageTemplate>
                            .Filter
                            .Eq(
                                o => o.QrCode,
                                qrCode
                            ),
                        cancellationToken
                    );
    }
}