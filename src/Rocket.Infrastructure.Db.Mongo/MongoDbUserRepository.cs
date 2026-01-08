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
    ) : IUserRepository
    {
        public async Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken) =>
            await
                GetUserByFilterAsync(
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Username,
                            username
                        ),
                    cancellationToken
                );

        public async Task<User> GetUserByUserIdAsync(string userId, CancellationToken cancellationToken) =>
            await
                GetUserByFilterAsync(
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Id,
                            userId
                        ),
                    cancellationToken
                );

        public async Task UpdateUserFieldAsync<T>(
            string userId,
            Expression<Func<User, T>> setter,
            T value,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var filter =
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Id,
                            userId
                        );

                var update =
                    Builders<User>
                        .Update
                        .Set(
                            setter,
                            value
                        );

                await
                    UpdateUserAsync
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
                        "Error updating last login for user {userId}: {error}",
                        userId,
                        ex.Message
                    );
                throw;
            }
        }
        
        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var userCollection =
                    mongoDatabase
                        .GetCollection<User>(MongoConstants.UserCollection);

                await
                    userCollection
                        .InsertOneAsync(
                            user,
                            new InsertOneOptions(),
                            cancellationToken
                        );

                return user;
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "Error creating user {username}: {error}",
                        user.Username,
                        ex.Message
                    );
                throw;
            }
        }

        public async Task DeactivateAdminUserAsync(CancellationToken cancellationToken)
        {
            try
            {
                var filter =
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Username,
                            DomainConstants.AdminUserName
                        );

                var update =
                    Builders<User>
                        .Update
                        .Set(
                            u => u.IsActive,
                            false
                        );

                await
                    UpdateUserAsync
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

        public async Task<(IEnumerable<User> records, long totalRecordCount)> FetchUsersAsync(
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

                var userCollection =
                    mongoDatabase
                        .GetCollection<User>(MongoConstants.UserCollection);

                var filter = 
                    Builders<User>
                        .Filter
                        .Empty;

                var totalRecordCount =
                    await
                        userCollection
                            .Find(filter)
                            .CountDocumentsAsync(cancellationToken: cancellationToken);

                var records =
                    await
                        userCollection
                            .Find(filter)
                            .SortByDescending(x => x.CreatedAt)
                            .Skip(startIndex)
                            .Limit(recordCount)
                            .ToListAsync(cancellationToken: cancellationToken);

                return (records, totalRecordCount);
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

        public async Task UpdateUserIsAdminAsync(
            string userId,
            bool value,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var filter =
                    Builders<User>
                        .Filter
                        .Eq(
                            u => u.Id,
                            userId
                        );

                var update =
                    Builders<User>
                        .Update
                        .Set(
                            u => u.IsAdmin,
                            value
                        );

                await
                    UpdateUserAsync
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
                        "Error updating admin status for user {userId}: {error}",
                        userId,
                        ex.Message
                    );
                throw;
            }
        }

        private async Task<User> GetUserByFilterAsync(
            FilterDefinition<User> filter,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var mongoDatabase =
                    mongoDbClient
                        .GetDatabase();

                var userCollection =
                    mongoDatabase
                        .GetCollection<User>(MongoConstants.UserCollection);

                return
                    await
                        userCollection
                            .Find(filter)
                            .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "Error retrieving user : {error}",
                        ex.Message
                    );
                throw;
            }
        }

        private async Task UpdateUserAsync(
            FilterDefinition<User> filter,
            UpdateDefinition<User> update,
            CancellationToken cancellationToken
        )
        {
            var mongoDatabase =
                mongoDbClient
                    .GetDatabase();

            var userCollection =
                mongoDatabase
                    .GetCollection<User>(MongoConstants.UserCollection);

            await
                userCollection
                    .UpdateOneAsync(
                        filter,
                        update,
                        new UpdateOptions(),
                        cancellationToken
                    );
        }
    }
}