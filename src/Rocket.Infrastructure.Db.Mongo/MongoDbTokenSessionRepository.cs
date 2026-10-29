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
    public class MongoDbTokenSessionRepository(
        ILogger<MongoDbTokenSessionRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<TokenSession>(
        mongoDbClient,
        logger
    ), ITokenSessionRepository
    {
        protected override string CollectionName => MongoConstants.TokenSessionsCollection;

        public async Task<TokenSession> InsertTokenAsync(
            TokenSession tokenSession,
            CancellationToken cancellationToken
        ) => await
            InsertRecordAsync(
                tokenSession,
                cancellationToken
            );

        public async Task<TokenSession> RotateAsync(
            string oldHash,
            string newHash,
            string newPrefix,
            DateTime now,
            DateTime newExpiresAt,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            return
                await
                    collection
                        .FindOneAndUpdateAsync(
                            t => t.Hash == oldHash && t.IsActive && t.ExpiresAt > now,
                            Builders<TokenSession>.Update
                                .Set(
                                    t => t.Hash,
                                    newHash
                                )
                                .Set(
                                    t => t.PreviousHash,
                                    oldHash
                                )
                                .Set(
                                    t => t.RotatedAt,
                                    now
                                )
                                .Set(
                                    t => t.TokenPrefix,
                                    newPrefix
                                )
                                .Set(
                                    t => t.LastLoginAt,
                                    now
                                )
                                .Set(
                                    t => t.ExpiresAt,
                                    newExpiresAt
                                ),
                            new FindOneAndUpdateOptions<TokenSession>
                            {
                                ReturnDocument = ReturnDocument.After
                            },
                            cancellationToken
                        );
        }

        public async Task RevokeIfReplayedAsync(
            string oldHash,
            DateTime now,
            TimeSpan grace,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            await
                collection
                    .UpdateOneAsync(
                        t => t.PreviousHash == oldHash && t.IsActive && t.RotatedAt < now - grace,
                        Builders<TokenSession>
                            .Update
                            .Set(
                                t => t.IsActive,
                                false
                            )
                            .Set(
                                t => t.RevokedAt,
                                now
                            ),
                        cancellationToken: cancellationToken
                    );
        }

        public async Task<IEnumerable<TokenSession>> GetActiveBySessionAsync(
            string sessionId,
            DateTime now,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            return
                await
                    collection
                        .Find(t => t.Id == sessionId && t.IsActive && t.ExpiresAt > now)
                        .SortByDescending(t => t.LastLoginAt)
                        .ToListAsync(cancellationToken);
        }        
        
        public async Task<IEnumerable<TokenSession>> GetActiveByUserAsync(
            string userId,
            DateTime now,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            return
                await
                    collection
                        .Find(t => t.UserId == userId && t.IsActive && t.ExpiresAt > now)
                        .SortByDescending(t => t.LastLoginAt)
                        .ToListAsync(cancellationToken);
        }

        public async Task<bool> RevokeAsync(
            string userId,
            string sessionId,
            DateTime now,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            var result =
                await
                    collection
                        .UpdateOneAsync(
                            t => t.Id == sessionId && t.UserId == userId && t.IsActive,
                            Builders<TokenSession>.Update
                                .Set(
                                    t => t.IsActive,
                                    false
                                )
                                .Set(
                                    t => t.RevokedAt,
                                    now
                                ),
                            cancellationToken: cancellationToken
                        );

            return
                result
                    .ModifiedCount > 0;
        }

        public async Task RevokeAllAsync(
            string userId,
            string exceptSessionId,
            DateTime now,
            CancellationToken cancellationToken
        )
        {
            var collection =
                GetMongoCollection();

            var f = Builders<TokenSession>.Filter;
            var filter = f.Eq(
                t => t.UserId,
                userId
            ) & f.Eq(
                t => t.IsActive,
                true
            );
            if (exceptSessionId != null)
                filter &= f.Ne(
                    t => t.Id,
                    exceptSessionId
                );

            await
                collection
                    .UpdateManyAsync(
                        filter,
                        Builders<TokenSession>.Update
                            .Set(
                                t => t.IsActive,
                                false
                            )
                            .Set(
                                t => t.RevokedAt,
                                now
                            ),
                        cancellationToken: cancellationToken
                    );
        }
    }
}