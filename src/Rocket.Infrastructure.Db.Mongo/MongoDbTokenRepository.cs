using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rocket.Domain;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Db.Mongo
{
    public class MongoDbTokenRepository(
        ILogger<MongoDbTokenRepository> logger,
        IMongoDbClient mongoDbClient
    ) : MongoDbRepositoryBase<Token>(
        mongoDbClient,
        logger
    ), ITokenRepository
    {
        protected override string CollectionName => MongoConstants.TokensCollection;

        public async Task<bool> DeleteTokenAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                DeleteFirstFilteredRecordAsync(
                    Builders<Token>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Token>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task<Token> InsertTokenAsync(
            Token token,
            CancellationToken cancellationToken
        ) =>
            await
                InsertRecordAsync(
                    token,
                    cancellationToken
                );

        public async Task<Token> GetTokenByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                FetchFirstFilteredRecordAsync(
                    Builders<Token>
                        .Filter
                        .Eq(
                            o => o.UserId,
                            userId
                        ) &
                    Builders<Token>
                        .Filter
                        .Eq(
                            o => o.Id,
                            id
                        ),
                    cancellationToken
                );

        public async Task UpdateTokenFieldAsync<TField>(
            string tokenId,
            string userId,
            Expression<Func<Token, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) =>
            await
                ApplyUpdateToFilteredRecordFieldAsync(
                    setter,
                    value,
                    Builders<Token>
                        .Filter
                        .Eq(
                            u => u.UserId,
                            userId
                        ) &
                    Builders<Token>
                        .Filter
                        .Eq(
                            o => o.Id,
                            tokenId
                        ),
                    cancellationToken
                );
    }
}