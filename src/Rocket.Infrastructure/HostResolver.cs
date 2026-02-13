using System;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class HostResolver(ILogger<HostResolver> logger) : IHostResolver
    {
        public string GetThisHost()
        {
            var publicBaseUrl =
                Environment
                    .GetEnvironmentVariable("PUBLIC_BASE_URL");

            if (string.IsNullOrEmpty(publicBaseUrl))
            {
                logger
                    .LogWarning(
                        "Cannot generate a QR code for the user account. An environment variable PUBLIC_BASE_URL needs to be supplied at runtime."
                    );
            }

            return publicBaseUrl;
        }
    }
}