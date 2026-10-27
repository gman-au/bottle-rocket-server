using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface ITokenRepository
    {
        Task<Token> GetTokenByIdAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<Token> InsertTokenAsync(
            Token token,
            CancellationToken cancellationToken
        );

        Task UpdateTokenFieldAsync<TField>(
            string tokenId,
            string userId,
            Expression<Func<Token, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        );
        
        Task<bool> DeleteTokenAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );
    }
}