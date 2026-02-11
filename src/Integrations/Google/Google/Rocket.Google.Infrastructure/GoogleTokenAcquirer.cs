using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Rocket.Google.Domain;

namespace Rocket.Google.Infrastructure
{
    public class GoogleTokenAcquirer : IGoogleTokenAcquirer
    {
        private const string DeviceCodeRedirect = "urn:ietf:wg:oauth:2.0:oob";
        private const string StaticUserId = "userId";

        public async Task<string> GetAuthorizationUrlAsync(
            GooglesCredential googlesCredential,
            CancellationToken cancellationToken
        )
        {
            var secrets =
                await
                    BuildSecretsAsync(
                        googlesCredential,
                        cancellationToken
                    );

            var codeFlow =
                new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = secrets,
                        Scopes = [DriveService.Scope.Drive]
                    }
                );

            var authUrl =
                codeFlow
                    .CreateAuthorizationCodeRequest(DeviceCodeRedirect);

            return
                authUrl
                    .Build()
                    .ToString();
        }

        public async Task<(string, string)> GetRefreshTokenFromAccessCodeAsync(
            GooglesCredential googlesCredential,
            string accessCode,
            CancellationToken cancellationToken
        )
        {
            var secrets =
                await
                    BuildSecretsAsync(
                        googlesCredential,
                        cancellationToken
                    );

            var tokenResponse =
                await
                    new GoogleAuthorizationCodeFlow(
                            new GoogleAuthorizationCodeFlow
                                .Initializer
                                {
                                    ClientSecrets = secrets,
                                    Scopes = [DriveService.Scope.Drive]
                                }
                        )
                        .ExchangeCodeForTokenAsync(
                            StaticUserId,
                            accessCode,
                            DeviceCodeRedirect,
                            cancellationToken
                        );

            return (
                tokenResponse.AccessToken,
                tokenResponse.RefreshToken
            );
        }

        public async Task<UserCredential> GetFlowCredentialAsync(
            GooglesCredential googlesCredential,
            string refreshToken,
            CancellationToken cancellationToken
        )
        {
            var secrets = await BuildSecretsAsync(
                googlesCredential,
                cancellationToken
            );

            var tokenResponse =
                new TokenResponse
                {
                    RefreshToken = refreshToken
                };

            // Create the flow
            var flow =
                new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = secrets,
                        Scopes = [DriveService.Scope.Drive]
                    }
                );

            // Create credential (will auto-refresh access token)
            return
                new UserCredential(
                    flow,
                    StaticUserId,
                    tokenResponse
                );
        }

        private static async Task<ClientSecrets> BuildSecretsAsync(
            GooglesCredential googlesCredential,
            CancellationToken cancellationToken
        )
        {
            // Convert your credential object back to JSON string
            var credentialJson =
                JsonSerializer
                    .Serialize(googlesCredential);

            using var stream =
                new MemoryStream(
                    Encoding
                        .UTF8
                        .GetBytes(credentialJson)
                );

            var secrets =
                (await
                    GoogleClientSecrets
                        .FromStreamAsync(
                            stream,
                            cancellationToken
                        )
                )
                .Secrets;

            return secrets;
        }
    }
}