using System;
using System.Threading.Tasks;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.MongoDb
{
    public class MongoDbScannedImageRepository(IMongoDbClient mongoDbClient) : IScannedImageRepository
    {
        public async Task<ScannedImage> SaveCaptureAsync()
        {
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            var scannedImageCollection =
                mongoDatabase
                    .GetCollection<ScannedImage>(MongoConstants.ScannedImageCollection);

            var scannedImage = new ScannedImage
            {
                CaptureDate = DateTime.UtcNow
            };

            await
                scannedImageCollection
                    .InsertOneAsync(scannedImage);

            return scannedImage;
        }
    }
}