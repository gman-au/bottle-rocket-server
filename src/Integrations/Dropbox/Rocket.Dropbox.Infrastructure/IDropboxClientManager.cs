using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Dropbox.Domain;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public interface IDropboxClientManager
    {
        string GetAuthorizeUrl(string appKey);

        Task<string> GetRefreshTokenFromAccessCodeAsync(
            string appKey,
            string appSecret,
            string accessCode
        );

        Task UploadFilesAsync(
            string appKey,
            string appSecret,
            string refreshToken,
            string uploadFolder,
            IEnumerable<DropboxFile> files, 
            IJobControl jobControl,
            CancellationToken cancellationToken
        );
    }
}