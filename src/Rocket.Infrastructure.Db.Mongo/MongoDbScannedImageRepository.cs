using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.ScannedImages;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbScannedImageRepository(
        ILogger<MongoDbScannedImageRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<ScannedImage>(
        mongoDbClient,
        logger
    ), IScannedImageRepository
    {
        protected override string CollectionName => MongoConstants.ScannedImageCollection;

        public async Task UpdateScannedImageFieldAsync<TField>(
            string userId,
            string id,
            Expression<Func<ScannedImage, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) =>
            await
                ApplyUpdateToFilteredRecordFieldAsync(
                    setter,
                    value,
                    Builders<ScannedImage>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        ) &
                    Builders<ScannedImage>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );
    }
}