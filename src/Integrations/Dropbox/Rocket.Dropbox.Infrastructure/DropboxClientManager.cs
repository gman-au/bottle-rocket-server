using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Dropbox.Domain;
using Rocket.Integrations.Domain;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxClientManager(
        ILogger<DropboxClientManager> logger
    ) : IDropboxClientManager
    {
        private const int MaxConcurrentUploads = 10;

        public string GetAuthorizeUrl(
            string appKey
        )
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
            try
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
            catch (Exception ex)
            {
                logger
                    .LogError(
                        "There was an error finalizing the Dropbox connection: {message}",
                        ex.Message
                    );

                throw new RocketException(
                    "There was an error finalizing the Dropbox connection.",
                    ApiStatusCodeEnum.ThirdPartyServiceError,
                    (int)HttpStatusCode.InternalServerError,
                    ex
                );
            }
        }

        public async Task<string> UploadFilesAsync(
            string appKey,
            string appSecret,
            string refreshToken,
            string uploadFolder,
            IEnumerable<DropboxFile> files,
            IJobControl jobControl,
            CancellationToken cancellationToken
        )
        {
            try
            {
                logger
                    .LogInformation("Uploading files to Dropbox");

                cancellationToken
                    .ThrowIfCancellationRequested();

                var dbx =
                    new DropboxClient(
                        refreshToken,
                        appKey,
                        appSecret
                    );

                using var throttle = new SemaphoreSlim(MaxConcurrentUploads);

                var tasks = files.Select(async file =>
                    {
                        await
                            throttle
                                .WaitAsync(cancellationToken);

                        try
                        {
                            using var stream = new MemoryStream(file.Artifact);

                            var path =
                                $"{((uploadFolder ?? string.Empty).StartsWith('/') ? null : '/')}{uploadFolder}" +
                                $"{(string.IsNullOrEmpty(uploadFolder) ? string.Empty : "/")}{file.FileName}{file.FileExtension}";

                            var result =
                                await
                                    dbx
                                        .Files
                                        .UploadAsync(
                                            path: path,
                                            mode: WriteMode.Overwrite.Instance,
                                            autorename: true,
                                            body: stream
                                        );

                            await
                                jobControl
                                    .AppendLogMessageCallback(
                                        new JobLogEntry
                                        {
                                            ScanId = file.ScanId,
                                            LogMessage = $"File uploaded: {file.FileName}{file.FileExtension}, Dropbox ID: {result.Id}"
                                        }
                                    );

                            await
                                jobControl
                                    .UpdateStepStatusCallback(
                                        new JobStatusEntry
                                        {
                                            ExecutionStatus = (int)ExecutionStatusEnum.Completed,
                                            ScanId = file.ScanId,
                                            Exception = null
                                        }
                                    );
                        }
                        catch (Exception ex)
                        {
                            await
                                jobControl
                                    .UpdateStepStatusCallback(
                                        new JobStatusEntry
                                        {
                                            ExecutionStatus = (int)ExecutionStatusEnum.Errored,
                                            ScanId = file.ScanId,
                                            Exception = ex
                                        }
                                    );
                        }
                        finally
                        {
                            throttle
                                .Release();
                        }
                    }
                );

                await
                    Task
                        .WhenAll(tasks);

                return
                    "TODO";
            }
            catch (DropboxException ex)
            {
                logger
                    .LogError(
                        "There was an error uploading the file to Dropbox: {message}",
                        ex.Message
                    );

                throw new RocketException(
                    $"There was an error uploading the file to Dropbox: {ex.Message}",
                    ApiStatusCodeEnum.ThirdPartyServiceError,
                    (int)HttpStatusCode.InternalServerError,
                    ex
                );
            }
        }
    }
}