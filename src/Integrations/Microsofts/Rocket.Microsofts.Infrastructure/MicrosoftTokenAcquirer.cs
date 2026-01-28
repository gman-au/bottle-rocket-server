using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class MicrosoftTokenAcquirer(
        ILogger<MicrosoftTokenAcquirer> logger,
        IConnectorRepository connectorRepository
    ) : IMicrosoftTokenAcquirer
    {
        private static readonly string[] Scopes = ["Files.ReadWrite.All", "offline_access"];

        public async Task<MicrosoftDeviceCodeResult> AcquireAccountIdentifierAsync(
            MicrosoftConnector connector,
            string userId,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(connector?.ClientId))
                throw new RocketException(
                    "No client ID was supplied for the connector",
                    ApiStatusCodeEnum.ValidationError
                );

            if (string.IsNullOrEmpty(connector?.TenantId))
                throw new RocketException(
                    "No client ID was supplied for the connector",
                    ApiStatusCodeEnum.ValidationError
                );

            var app =
                PublicClientApplicationBuilder
                    .Create(connector.ClientId)
                    .WithAuthority(
                        AzureCloudInstance.AzurePublic,
                        connector.TenantId
                    )
                    .Build();

            DeviceCodeResult deviceCodeResult = null;

            // Start the auth process but don't await it yet
            var authTask =
                app
                    .AcquireTokenWithDeviceCode(
                        Scopes,
                        dcr =>
                        {
                            deviceCodeResult = dcr;
                            return Task.CompletedTask;
                        }
                    )
                    .ExecuteAsync(CancellationToken.None);

            // Wait for device code to be set
            while (deviceCodeResult == null)
            {
                await
                    Task
                        .Delay(
                            100,
                            CancellationToken.None
                        );
            }

            // Return the device code info to the user
            var result = new MicrosoftDeviceCodeResult
            {
                UserCode = deviceCodeResult.UserCode,
                DeviceCode = deviceCodeResult.DeviceCode,
                VerificationUrl = deviceCodeResult.VerificationUrl,
                ExpiresIn = deviceCodeResult.ExpiresOn.Offset.TotalSeconds,
                Interval = deviceCodeResult.Interval,
                Message = deviceCodeResult.Message
            };

            _ = Task.Run(
                async () =>
                {
                    try
                    {
                        var authResult = await authTask;

                        logger
                            .LogInformation("Token acquired successfully");

                        var accountIdentifier =
                            authResult
                                .Account
                                .HomeAccountId
                                .Identifier;

                        await
                            connectorRepository
                                .UpdateConnectorFieldAsync<MicrosoftConnector, string>(
                                    connector.Id,
                                    userId,
                                    o => o.AccountIdentifier,
                                    accountIdentifier,
                                    CancellationToken.None
                                );
                    }
                    catch (Exception ex)
                    {
                        logger
                            .LogError(
                                ex,
                                "Failed to complete authentication"
                            );
                    }
                }
            );

            return result;
        }

        public async Task<string> AcquireTokenSilentAsync(
            MicrosoftConnector connector,
            CancellationToken cancellationToken
        )
        {
            var app = PublicClientApplicationBuilder
                .Create(connector.ClientId)
                .WithAuthority(
                    AzureCloudInstance.AzurePublic,
                    connector.TenantId
                )
                .Build();

            var accounts =
                await
                    app
                        .GetAccountsAsync();

            var account =
                accounts
                    .FirstOrDefault(a => a.HomeAccountId.Identifier == connector.AccountIdentifier);

            var result =
                await
                    app
                        .AcquireTokenSilent(
                            Scopes,
                            account
                        )
                        .ExecuteAsync(cancellationToken);

            return result.AccessToken;
        }
    }
}