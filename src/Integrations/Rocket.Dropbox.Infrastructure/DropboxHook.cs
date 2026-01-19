using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Connectors;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxHook(
        IDropboxClientManager dropboxClientManager,
        IConnectorRepository connectorRepository
    ) : IIntegrationHook
    {
        public async Task ProcessAsync(
            string userId,
            byte[] fileData,
            string fileExtension,
            CancellationToken cancellationToken
        )
        {
            var connector =
                await
                    connectorRepository
                        .GetConnectorByNameAsync<DropboxConnector>(
                            userId,
                            DomainConstants.ConnectorNameDropboxApi,
                            cancellationToken
                        );

            if (connector == null) 
                return;
            
            await
                dropboxClientManager
                    .UploadFileAsync(
                        connector.AppKey,
                        connector.AppSecret,
                        connector.RefreshToken,
                        fileExtension,
                        fileData
                    );
        }
    }
}