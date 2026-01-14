using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbScannedImageRepository(
        ILogger<MongoDbScannedImageRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<ScannedImage>(mongoDbClient, logger), IScannedImageRepository
    {
        protected override string CollectionName => MongoConstants.ScannedImageCollection;

        public async Task<ScannedImage> SaveCaptureAsync(
            ScannedImage scannedImage,
            CancellationToken cancellationToken) =>
            await
                InsertRecordAsync(scannedImage, cancellationToken);

        public async Task<(IEnumerable<ScannedImage> records, long totalRecordCount)> FetchScansAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        ) => await
            FetchAllPagedAndFilteredRecordsAsync(
                startIndex,
                recordCount,
                Builders<ScannedImage>
                    .Filter
                    .Eq(
                        u => u.UserId,
                        userId
                    ),
                o => o.CaptureDate,
                cancellationToken
            );

        public async Task<ScannedImage> FetchScanAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<ScannedImage>
                        .Filter
                        .Eq(
                            o => o.UserId,
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