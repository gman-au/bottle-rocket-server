using System;
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
    ) : IScannedImageRepository
    {
        public async Task<ScannedImage> SaveCaptureAsync(ScannedImage scannedImage, CancellationToken cancellationToken)
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var scannedImageCollection =
                    mongoDatabase
                        .GetCollection<ScannedImage>(MongoConstants.ScannedImageCollection);

                await
                    scannedImageCollection
                        .InsertOneAsync(
                            scannedImage,
                            new InsertOneOptions(),
                            cancellationToken
                        );

                return scannedImage;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error saving the scanned image: {error}",
                        ex.Message
                    );

                throw;
            }
        }

        public async Task<(IEnumerable<ScannedImage> records, long totalRecordCount)> SearchScansAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var scannedImageCollection =
                    mongoDatabase
                        .GetCollection<ScannedImage>(MongoConstants.ScannedImageCollection);

                var filter =
                    Builders<ScannedImage>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        );

                var totalRecordCount =
                    await
                    scannedImageCollection
                        .Find(filter)
                        .CountDocumentsAsync(cancellationToken:cancellationToken);

                var records =
                    await
                        scannedImageCollection
                            .Find(filter)
                            .SortByDescending(x => x.CaptureDate)
                            .Skip(startIndex)
                            .Limit(recordCount)
                            .ToListAsync(cancellationToken: cancellationToken);

                return (records, totalRecordCount);
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error saving the scanned image: {error}",
                        ex.Message
                    );

                throw;
            }
        }
    }
}