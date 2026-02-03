using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class GraphClientProvider(IMicrosoftTokenAcquirer tokenAcquirer) : IGraphClientProvider
    {
        public async Task<GraphServiceClient> GetClientAsync(
            MicrosoftConnector connector,
            CancellationToken cancellationToken
        )
        {
            var accessToken =
                await
                    tokenAcquirer
                        .AcquireTokenSilentAsync(
                            connector,
                            cancellationToken
                        );
            
            var authProvider =
                new DelegateAuthenticationProvider(
                    requestMessage =>
                    {
                        requestMessage.Headers.Authorization =
                            new
                                System.Net.Http.Headers.AuthenticationHeaderValue(
                                    "Bearer",
                                    accessToken
                                );

                        return
                            Task
                                .CompletedTask;
                    }
                );

            return
                new GraphServiceClient(authProvider);
        }
    }
}