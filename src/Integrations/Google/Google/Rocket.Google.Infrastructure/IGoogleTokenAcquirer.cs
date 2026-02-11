using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Rocket.Google.Domain;

namespace Rocket.Google.Infrastructure
{
    public interface IGoogleTokenAcquirer
    {
        Task<string> GetAuthorizationUrlAsync(
            GooglesCredential googlesCredential,
            CancellationToken cancellationToken
        );

        Task<(string, string)> GetRefreshTokenFromAccessCodeAsync(
            GooglesCredential googlesCredential,
            string accessCode,
            CancellationToken cancellationToken
        );

        Task<UserCredential> GetFlowCredentialAsync(
            GooglesCredential googlesCredential,
            string refreshToken,
            CancellationToken cancellationToken
        );
    }
}