using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Rocket.Domain.Batches;
using Rocket.Domain.Dashboard;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.ScannedImages;
using Rocket.Domain.Utils;
using Rocket.Infrastructure.Db.Mongo.Extensions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbBatchRepository(
        ILogger<MongoDbScannedImageRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<Batch>(
        mongoDbClient,
        logger
    ), IBatchRepository
    {
        protected override string CollectionName => MongoConstants.BatchCollection;


        public async Task<Batch> GetBatchByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<Batch>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Batch>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task<(IEnumerable<Batch> records, long totalRecordCount)> FetchBatchesAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        ) => await
            FetchAllPagedAndFilteredRecordsAsync(
                startIndex,
                recordCount,
                Builders<Batch>
                    .Filter
                    .Eq(
                        u => u.UserId,
                        userId
                    ) &
                Builders<Batch>
                    .Filter
                    .Ne(
                        u => u.Status,
                        (int)BatchStatusEnum.Archived
                    ) &
                Builders<Batch>
                    .Filter
                    .SizeGt(
                        u => u.Scans,
                        0
                    ),
                o => o.CaptureDate,
                cancellationToken
            );

        public async Task<Batch> InsertBatchAsync(
            Batch batch,
            CancellationToken cancellationToken
        ) =>
            await
                InsertRecordAsync(
                    batch,
                    cancellationToken
                );

        public async Task ArchiveBatchAsync(
            string userId,
            string batchId,
            CancellationToken cancellationToken
        )
        {
            await
                ApplyUpdateToFilteredRecordFieldAsync(
                    o => o.Status,
                    (int)BatchStatusEnum.Archived,
                    Builders<Batch>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Batch>
                        .Filter
                        .Eq(
                            o => o.Id,
                            batchId
                        ),
                    cancellationToken
                );
        }

        public async Task DeleteBatchAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        )
        {
            await
                DeleteFirstFilteredRecordAsync(
                    Builders<Batch>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Batch>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );
        }

        public async Task<IEnumerable<ScanByVendorTotal>> AggregateScansByVendorAsync(
            string userId,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            var filter = Builders<Batch>
                .Filter
                .Eq(
                    o => o.UserId,
                    userId
                );

            var result =
                await
                    collection
                        .Aggregate()
                        .Match(filter)
                        .Unwind(o => o.Scans)
                        .ReplaceRoot<ScannedImage>("$Scans")
                        .Group(
                            o => o.Vendor,
                            g =>
                                new ScanByVendorTotal
                                {
                                    Vendor = g.Key ?? DomainConstants.UnknownType,
                                    Scans = g.Count()
                                }
                        )
                        .ToListAsync(cancellationToken);

            return result;
        }

        public async Task UpdateBatchFieldAsync<TField>(
            string userId,
            string id,
            Expression<Func<Batch, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) =>
            await
                ApplyUpdateToFilteredRecordFieldAsync(
                    setter,
                    value,
                    Builders<Batch>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        ) &
                    Builders<Batch>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task<ScannedImage> InsertBatchScanAsync(
            ScannedImage scannedImage,
            string userId,
            string batchId,
            CancellationToken cancellationToken
        )
        {
            var scanId =
                ObjectId
                    .GenerateNewId()
                    .ToString();

            scannedImage.Id = scanId;

            var filter =
                BuildBatchFilterByIdAndUserId(
                    batchId,
                    userId
                );

            var options = new UpdateOptions();

            var batch =
                await
                    GetBatchByIdAsync(
                        userId,
                        batchId,
                        cancellationToken
                    );

            if (batch == null)
                throw new RocketException(
                    $"Could not find batch with id [{batchId}]",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var update =
                Builders<Batch>
                    .Update
                    .Push(
                        w => w.Scans,
                        scannedImage
                    );

            await
                GetMongoCollection()
                    .UpdateOneAsync(
                        filter,
                        update,
                        options,
                        cancellationToken
                    );

            return scannedImage;
        }

        public async Task<ScannedImage> GetScanByIdAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            var batch =
                await
                    GetBatchByIdAsync(
                        userId,
                        batchId,
                        cancellationToken
                    );

            return
                (batch?.Scans)
                .FindScanById(
                    scanId
                );
        }


        public async Task DeleteScanByIdAsync(
            string userId,
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            var batch =
                await
                    GetBatchByIdAsync(
                        userId,
                        batchId,
                        cancellationToken
                    );

            batch
                .DeleteScanById(scanId);

            await
                UpdateBatchFieldAsync(
                    userId,
                    batchId,
                    o => o.Scans,
                    batch.Scans,
                    cancellationToken
                );
        }

        private static FilterDefinition<Batch> BuildBatchFilterByIdAndUserId(
            string batchId,
            string userId
        ) =>
            Builders<Batch>
                .Filter
                .Eq(
                    w => w.Id,
                    batchId
                ) &
            Builders<Batch>
                .Filter
                .Eq(
                    w => w.UserId,
                    userId
                );
    }
}