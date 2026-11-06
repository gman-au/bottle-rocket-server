using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain.Batches;
using Rocket.Domain.Dashboard;
using Rocket.Domain.Enum;
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
            string id,
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
                            id
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
//TODO: aggregate children
            /*
            var filter = Builders<ScannedImage>
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
                        .Group(
                            o => o.Vendor,
                            g => new ScanByVendorTotal
                            {
                                Vendor = g.Key ?? DomainConstants.UnknownType,
                                Scans = g.Count()
                            }
                        )
                        .ToListAsync(cancellationToken);

            return result;
            */
            return [];
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
    }
}