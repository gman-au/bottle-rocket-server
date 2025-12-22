using System;
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
    }
}