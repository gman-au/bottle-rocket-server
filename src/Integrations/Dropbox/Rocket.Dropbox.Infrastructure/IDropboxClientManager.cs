using System.Threading;
using System.Threading.Tasks;

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

        Task<bool> UploadFileAsync(
            string appKey,
            string appSecret,
            string refreshToken,
            string uploadFolder,
            string fileExtension,
            byte[] fileData, 
            CancellationToken cancellationToken
        );
    }
}