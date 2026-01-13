using System.Threading.Tasks;

namespace Rocket.Integrations.Dropbox
{
    public interface IDropboxClientManager
    {
        string GetAuthorizeUrl(string appKey);

        Task<string> GetRefreshTokenAsync(
            string appKey,
            string appSecret,
            string accessCode
        );

        Task<bool> UploadFileAsync(byte[] fileData);
    }
}