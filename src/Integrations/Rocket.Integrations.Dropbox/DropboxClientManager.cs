using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;

namespace Rocket.Integrations.Dropbox
{
    public class DropboxClientManager(ILogger<DropboxClientManager> logger) : IDropboxClientManager
    {
        private const string UploadSubfolder = "bottle_rocket_scans";
        
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

        public async Task<string> GetRefreshTokenFromAccessCodeAsync(
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
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            return
                tokenResult
                    .RefreshToken;
        }

        public async Task<bool> UploadFileAsync(
            string appKey,
            string appSecret,
            string refreshToken,
            string fileExtension,
            byte[] fileData
        )
        {
            try
            {
                logger
                    .LogInformation("Uploading file to Dropbox");
                
                var dbx =
                    new DropboxClient(
                        refreshToken,
                        appKey,
                        appSecret
                    );

                using var stream = new MemoryStream(fileData);

                await
                    dbx
                        .Files
                        .UploadAsync(
                            new UploadArg(path: $"/{UploadSubfolder}/{Guid.NewGuid()}{fileExtension}"),
                            stream
                        );

                return true;
            }
            catch (DropboxException ex)
            {
                logger
                    .LogError("There was an error uploading the file to Dropbox: {message}", ex.Message);

                throw new RocketException(
                    "There was an error uploading the file to Dropbox.",
                    ApiStatusCodeEnum.ThirdPartyServiceError,
                    (int)HttpStatusCode.InternalServerError,
                    ex
                );
            }
        }
    }
}