using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;

namespace Rocket.Google.Infrastructure
{
    public class DriveFolderSearcher(IGoogleTokenAcquirer tokenAcquirer) : IDriveFolderSearcher
    {
        public async Task<IEnumerable<GoogleDriveFolder>> GetFoldersAsync(
            GoogleConnector googleConnector,
            CancellationToken cancellationToken
        )
        {
            var googlesCredential =
                googleConnector
                    .Credential;

            if (googlesCredential == null)
                throw new RocketException(
                    "Credential file is not supplied for operation, please check Google connector",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            var refreshToken =
                googleConnector
                    .RefreshToken;

            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new RocketException(
                    "Refresh token is not supplied for operation, please check Google connector",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );

            var credential =
                await tokenAcquirer
                    .GetFlowCredentialAsync(
                        googlesCredential,
                        refreshToken,
                        cancellationToken
                    );

            var service =
                new DriveService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential
                    }
                );

            const string query = "mimeType = 'application/vnd.google-apps.folder' " +
                // "and 'root' in parents " +
                "and trashed = false";

            var listRequest = service.Files.List();
            listRequest.Q = query;
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            var folders = new List<File>();
            string nextPageToken = null;

            do
            {
                listRequest.PageToken = nextPageToken;

                var fileList =
                    await
                        listRequest
                            .ExecuteAsync(cancellationToken);

                if (fileList.Files != null)
                {
                    foreach (var file in fileList.Files)
                    {
                        folders
                            .Add(file);
                    }
                }

                nextPageToken =
                    fileList
                        .NextPageToken;
            } while (!string.IsNullOrEmpty(nextPageToken));

            return
                folders
                    .Select(
                        o =>
                            new GoogleDriveFolder
                            {
                                FolderId = o.Id,
                                FolderName = o.Name
                            }
                    );
        }
    }
}