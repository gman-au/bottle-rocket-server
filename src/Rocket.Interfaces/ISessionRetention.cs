using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ISessionRetention
    {
        string CachedAuthHeader { get; }
        
        string CachedRefreshToken { get; }
        
        string CachedUserName { get; }

        string CachedRole { get; }

        string CachedCurrentLanguage { get; }

        DateTime? CachedExpiresAt { get; }

        Task RetainSessionUserAsync(
            string userName,
            string role,
            string currentLanguage,
            CancellationToken cancellationToken
        );

        Task RetainSessionTokensAsync(
            string accessToken,
            string refreshToken,
            DateTime accessTokenExpiresAt,
            CancellationToken cancellationToken
        );

        Task ClearLocalStateAsync();

        Task InitializeStoredSessionAsync();

        bool IsInitialized();

        void SetCurrentLanguage(
            string languageCode
        );
    }
}