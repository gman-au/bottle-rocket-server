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

        public async Task<IEnumerable<ScannedImage>> SearchScansAsync(
            string userId,
            int currentPage,
            int pageSize,
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

                var skipAmount = (currentPage - 1) * pageSize;

                var findFluent =
                    scannedImageCollection
                        .Find(filter)
                        .SortBy(x => x.CaptureDate)
                        .Skip(skipAmount)
                        .Limit(pageSize);

                var pagedResults =
                    await
                        findFluent
                            .ToListAsync(cancellationToken: cancellationToken);

                return pagedResults;
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