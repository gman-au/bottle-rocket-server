using System.Threading.Tasks;
using Rocket.Interfaces;

namespace Rocket.Integrations.Dropbox
{
    public class DropboxHook(
        IDropboxClientManager dropboxClientManager
        ) : IIntegrationHook
    {
        public async Task ProcessAsync(byte[] fileData)
        {
            await 
                dropboxClientManager
                    .UploadFileAsync(fileData);
        }
    }
}