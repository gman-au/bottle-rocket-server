using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbUserRepository(
        ILogger<MongoDbUserRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<User>(mongoDbClient, logger), IUserRepository
    {
        protected override string CollectionName => MongoConstants.UserCollection;

        public async Task<(IEnumerable<User> records, long totalRecordCount)> FetchUsersAsync(
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        ) => await
            FetchAllPagedAndFilteredRecordsAsync(
                startIndex,
                recordCount,
                null,
                o => o.CreatedAt,
                cancellationToken
            );

        public async Task<User> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken
        ) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Username,
                            username
                        ),
                    cancellationToken
                );

        public async Task<User> GetUserByUserIdAsync(
            string userId,
            CancellationToken cancellationToken) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Id,
                            userId
                        ),
                    cancellationToken
                );

        public async Task<User> CreateUserAsync(
            User user,
            CancellationToken cancellationToken) =>
            await
                InsertRecordAsync(user, cancellationToken);

        public async Task UpdateUserFieldAsync<T>(
            string userId,
            Expression<Func<User, T>> setter,
            T value,
            CancellationToken cancellationToken) =>
            await
                ApplyUpdateToFilteredRecordFieldAsync(
                    setter,
                    value,
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Id,
                            userId
                        ),
                    cancellationToken
                );

        public async Task DeactivateAdminUserAsync(CancellationToken cancellationToken)
        {
            try
            {
                var filter =
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Username,
                            DomainConstants.RootAdminUserName
                        );

                var update =
                    Builders<User>
                        .Update
                        .Set(
                            u => u.IsActive,
                            false
                        );

                await
                    ApplyUpdateToFilteredRecordAsync
                    (
                        filter,
                        update,
                        cancellationToken
                    );

                logger
                    .LogWarning("Admin account has been deactivated");
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        ex,
                        "Error deactivating admin account"
                    );
                throw;
            }
        }

        public async Task<(IEnumerable<User> records, long totalRecordCount)> GetActiveAdminsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var filter =
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.IsActive,
                            true
                        );

                filter &=
                    Builders<User>
                        .Filter
                        .Eq(
                            o => o.IsAdmin,
                            true
                        );

                // do not count the root admin account
                filter &=
                    Builders<User>
                        .Filter
                        .Ne(
                            o => o.Username,
                            DomainConstants.RootAdminUserName
                        );

                return
                    await
                        FetchAllPagedAndFilteredRecordsAsync(
                            filter: filter,
                            cancellationToken: cancellationToken
                        );
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an fetching users: {error}",
                        ex.Message
                    );

                throw;
            }
        }
    }
}