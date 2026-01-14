using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Rocket.Infrastructure.Db.Mongo
{
    public abstract class MongoDbRepositoryBase<T>(
        IMongoDbClient mongoDbClient,
        ILogger logger
    )
    {
        protected abstract string CollectionName { get; }

        protected async Task ApplyUpdateToFilteredRecordAsync(
            FilterDefinition<T> filter,
            UpdateDefinition<T> update,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            await
                collection
                    .UpdateOneAsync(
                        filter,
                        update,
                        new UpdateOptions(),
                        cancellationToken
                    );
        }

        protected async Task ApplyUpdateToFilteredRecordFieldAsync<TField>(
            Expression<Func<T, TField>> setter,
            TField value,
            FilterDefinition<T> filter = null,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                filter ??=
                    Builders<T>
                        .Filter
                        .Empty;

                var update =
                    Builders<T>
                        .Update
                        .Set(
                            setter,
                            value
                        );

                await
                    ApplyUpdateToFilteredRecordAsync
                    (
                        filter,
                        update,
                        cancellationToken
                    );
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "Error updating ({type}) record field : {error}",
                        typeof(T).Name,
                        ex.Message
                    );
                throw;
            }
        }

        protected async Task<T> InsertRecordAsync(
            T record,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await
                    GetMongoCollection()
                        .InsertOneAsync(
                            record,
                            new InsertOneOptions(),
                            cancellationToken
                        );

                return record;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error saving the ({type}) record: {error}",
                        typeof(T).Name,
                        ex.Message
                    );

                throw;
            }
        }

        protected async Task<(IEnumerable<T> records, long totalRecordCount)> FetchAllPagedAndFilteredRecordsAsync(
            int? startIndex = null,
            int? recordCount = null,
            FilterDefinition<T> filter = null,
            Expression<Func<T, object>> orderBy = null,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var collection =
                    GetMongoCollection();

                filter ??=
                    Builders<T>
                        .Filter
                        .Empty;

                var totalRecordCount =
                    await
                        collection
                            .Find(filter)
                            .CountDocumentsAsync(cancellationToken);

                var findFluent =
                    collection
                        .Find(filter);

                if (orderBy != null)
                    findFluent
                        .SortByDescending(orderBy);

                var records =
                    await
                        findFluent
                            .Skip(startIndex)
                            .Limit(recordCount)
                            .ToListAsync(cancellationToken);

                return (records, totalRecordCount);
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an fetching ({type}) records: {error}",
                        typeof(T).Name,
                        ex.Message
                    );

                throw;
            }
        }

        protected async Task<T> FetchFirstFilteredRecordAsync(
            FilterDefinition<T> filter,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var collection =
                    GetMongoCollection();

                return
                    await
                        collection
                            .Find(filter)
                            .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "Error retrieving ({type}) : {error}",
                        typeof(T).Name,
                        ex.Message
                    );
                throw;
            }
        }

        protected async Task<bool> DeleteFirstFilteredRecordAsync(
            FilterDefinition<T> filter,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            var record =
                await
                    collection
                        .DeleteOneAsync(
                            filter,
                            cancellationToken
                        );

            return
                record
                    .DeletedCount > 0;
        }

        protected IMongoCollection<TCustom> GetMongoCollection<TCustom>()
        {
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            return
                mongoDatabase
                    .GetCollection<TCustom>(CollectionName);
        }

        private IMongoCollection<T> GetMongoCollection()
        {
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            return
                mongoDatabase
                    .GetCollection<T>(CollectionName);
        }
    }
}