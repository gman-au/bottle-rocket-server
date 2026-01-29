using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
        ICaptureNotifier captureNotifier,
        IConnectorRepository connectorRepository
    ) : IMicrosoftTokenAcquirer
    {
        private static readonly string[] Scopes = ["Files.ReadWrite.All", "offline_access", "Notes.ReadWrite.All"];
        private const string AuthEndpoint = "https://login.microsoftonline.com/{0}/oauth2/v2.0/token";

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

            if (string.IsNullOrEmpty(connector.TenantId))
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
                        using var cts =
                            new CancellationTokenSource(
                                TimeSpan
                                    .FromMinutes(3)
                            );

                        await
                            authTask
                                .WaitAsync(cts.Token);

                        logger
                            .LogInformation("Token acquired successfully");

                        var cacheData =
                            (app.UserTokenCache as ITokenCacheSerializer)?
                            .SerializeMsalV3();

                        var cacheJson =
                            System
                                .Text
                                .Encoding
                                .UTF8
                                .GetString(cacheData);

                        var cacheObject =
                            JsonDocument
                                .Parse(cacheJson);

                        var refreshTokens =
                            cacheObject
                                .RootElement
                                .GetProperty("RefreshToken");

                        var refreshToken =
                            refreshTokens
                                .EnumerateObject()
                                .First()
                                .Value
                                .GetProperty("secret")
                                .GetString();

                        await
                            connectorRepository
                                .UpdateConnectorFieldAsync<MicrosoftConnector, string>(
                                    connector.Id,
                                    userId,
                                    o => o.RefreshToken,
                                    refreshToken,
                                    CancellationToken.None
                                );

                        await
                            captureNotifier
                                .NotifyConnectorUpdateAsync(
                                    userId,
                                    success: true,
                                    cancellationToken
                                );
                    }
                    catch (OperationCanceledException)
                    {
                        logger
                            .LogError("Authentication timed out");

                        await
                            captureNotifier
                                .NotifyConnectorUpdateAsync(
                                    userId,
                                    success: false,
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

                        await
                            captureNotifier
                                .NotifyConnectorUpdateAsync(
                                    userId,
                                    success: false,
                                    CancellationToken.None
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
            using var httpClient = new HttpClient();

            var requestBody =
                new FormUrlEncodedContent(
                    [
                        new KeyValuePair<string, string>(
                            "client_id",
                            connector.ClientId
                        ),
                        new KeyValuePair<string, string>(
                            "refresh_token",
                            connector.RefreshToken
                        ),
                        new KeyValuePair<string, string>(
                            "grant_type",
                            "refresh_token"
                        ),
                        new KeyValuePair<string, string>(
                            "scope",
                            string.Join(
                                " ",
                                Scopes
                            )
                        )
                    ]
                );

            var response =
                await
                    httpClient
                        .PostAsync(
                            string.Format(
                                AuthEndpoint,
                                connector.TenantId
                            ),
                            requestBody,
                            cancellationToken
                        );

            response
                .EnsureSuccessStatusCode();

            var json =
                await
                    response
                        .Content
                        .ReadAsStringAsync(cancellationToken);

            var tokenResponse =
                JsonDocument
                    .Parse(json);

            var accessToken =
                tokenResponse
                    .RootElement
                    .GetProperty("access_token")
                    .GetString();

            var newRefreshToken =
                tokenResponse
                    .RootElement
                    .GetProperty("refresh_token")
                    .GetString();

            // Update refresh token if changed
            if (newRefreshToken != connector.RefreshToken)
            {
                await
                    connectorRepository
                        .UpdateConnectorFieldAsync<MicrosoftConnector, string>(
                            connector.Id,
                            connector.UserId,
                            o => o.RefreshToken,
                            newRefreshToken,
                            CancellationToken.None
                        );
            }

            return accessToken;
        }
    }
}