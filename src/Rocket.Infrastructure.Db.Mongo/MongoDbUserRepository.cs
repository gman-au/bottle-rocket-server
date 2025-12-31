using System;
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
        public async Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
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
                        .Eq(
                            u => u.Username,
                            username
                        );

                return
                    await userCollection
                        .Find(filter)
                        .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Error retrieving user {username}: {error}",
                    username,
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

        public async Task UpdateLastLoginAsync(string userId, CancellationToken cancellationToken)
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
                        .Eq(
                            u => u.Id,
                            userId
                        );
                var update =
                    Builders<User>
                        .Update
                        .Set(
                            u => u.LastLoginAt,
                            DateTime.UtcNow
                        );

                await
                    userCollection
                        .UpdateOneAsync(
                            filter,
                            update,
                            new UpdateOptions(),
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
        
        public async Task DeactivateAdminUserAsync(CancellationToken cancellationToken)
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
                    userCollection
                        .UpdateOneAsync(
                            filter,
                            update,
                            new UpdateOptions(),
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
    }
}