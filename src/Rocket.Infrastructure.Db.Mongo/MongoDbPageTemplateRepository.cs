using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.PageTemplates;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbPageTemplateRepository(
        ILogger<MongoDbPageTemplateRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<PageTemplate>(mongoDbClient, logger), IPageTemplateRepository
    {
        protected override string CollectionName => MongoConstants.PageTemplateCollection;

        public async Task<long> UpsertPageTemplateAsync(
            PageTemplate pageTemplate,
            CancellationToken cancellationToken)
        {
            try
            {
                var filter =
                    Builders<PageTemplate>
                        .Filter
                        .Eq(o => o.QrCode, pageTemplate.QrCode);

                var update =
                    Builders<PageTemplate>
                        .Update
                        .Set(o => o.PaperSizeType, pageTemplate.PaperSizeType)
                        .Set(o => o.QrCodeOrientationType, pageTemplate.QrCodeOrientationType)
                        .Set(o => o.RocketbookPageTemplateType, pageTemplate.RocketbookPageTemplateType)
                        .Set(o => o.SymbolsBoundingBox, pageTemplate.SymbolsBoundingBox);

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
                        nameof(PageTemplate),
                        ex.Message
                    );

                throw;
            }
        }
    }
}