using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Rocket.Interfaces;

namespace Rocket.Api.Host
{
    public class StartupInitializationHostedService(IStartupInitialization initService) : IHostedService
    {
        private const string OpenApiBuildArg = "OpenApiGenerateDocuments";
        private const string GetDocumentArg = "GetDocument.Insider";

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var args =
                Environment
                    .GetCommandLineArgs();

            var isBuildTime =
                args
                    .Any(a =>
                        a.Contains(OpenApiBuildArg) ||
                        a.Contains(GetDocumentArg)
                    );

            if (!isBuildTime)
            {
                await
                    initService
                        .InitializeAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}