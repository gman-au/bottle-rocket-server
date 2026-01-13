using System.Threading.Tasks;
using Dropbox.Api;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;

namespace Rocket.Integrations.Dropbox
{
    public class DropboxClientManager : IDropboxClientManager
    {
        public string GetAuthorizeUrl(string appKey)
        {
            var authorizeUri =
                DropboxOAuth2Helper
                    .GetAuthorizeUri(
                        OAuthResponseType.Code,
                        appKey,
                        (string)null,
                        tokenAccessType: TokenAccessType.Offline
                    );

            return
                authorizeUri
                    .ToString();
        }

        public async Task<string> GetRefreshTokenAsync(
            string appKey,
            string appSecret,
            string accessCode
        )
        {
            var tokenResult =
                await
                    DropboxOAuth2Helper
                        .ProcessCodeFlowAsync(
                            accessCode,
                            appKey,
                            appSecret
                        );
            
            var refreshToken = 
                tokenResult?
                    .RefreshToken;

            if (string.IsNullOrEmpty(refreshToken))
                throw new RocketException(
                    "A valid refresh token was not returned from Dropbox. Make sure you have configured the app correctly.",
                    ApiStatusCodeEnum.ServerConfigurationError
                );

            return
                tokenResult
                    .RefreshToken;
        }

        public async Task<bool> UploadFileAsync(byte[] fileData)
        {
            return true;
        }
    }
}