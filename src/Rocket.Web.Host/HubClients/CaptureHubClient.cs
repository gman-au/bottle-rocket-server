using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Interfaces;
using Rocket.Web.Client.Options;

namespace Rocket.Web.Host.HubClients
{
    public class CaptureHubClient(
        ILogger<CaptureHubClient> logger,
        IOptions<ApiConfigurationOptions> apiConfigurationOptionsAccessor,
        IAuthenticationManager authenticationManager
    ) : ICaptureHubClient
    {
        private readonly ApiConfigurationOptions _options = apiConfigurationOptionsAccessor.Value;
        private HubConnection _hubConnection;

        public event Func<Task> OnNewCaptureReceived;

        public event Func<Task> OnNewExecutionUpdateReceived;

        public event Func<bool, Task> OnNewConnectorUpdateReceived;

        public bool IsConnected =>
            _hubConnection?.State == HubConnectionState.Connected;

        private readonly List<IDisposable> _hubSubscriptions = [];
        
        public async Task StartAsync()
        {
            try
            {
                if (_hubConnection != null)
                {
                    await StopAsync();
                }

                var authHeader =
                    await
                        authenticationManager
                            .GetAuthorizationHeaderAsync();

                var apiBaseUrl =
                    _options?.BaseUrl ??
                    throw new ConfigurationErrorsException(
                        nameof(_options.BaseUrl)
                    );

                _hubConnection =
                    new HubConnectionBuilder()
                        .WithUrl(
                            $"{apiBaseUrl}/hubs/capture",
                            options =>
                            {
                                options.HttpMessageHandlerFactory = handler =>
                                    new AuthHeaderHandler(authHeader)
                                    {
                                        InnerHandler = handler
                                    };
                            }
                        )
                        .WithAutomaticReconnect()
                        .Build();

                foreach (var sub in _hubSubscriptions)
                    sub
                        .Dispose();

                _hubSubscriptions
                    .Clear();

                _hubSubscriptions
                    .Add(
                        _hubConnection
                            .On(
                                "NewCaptureReceived",
                                async () =>
                                {
                                    logger
                                        .LogInformation("Received NewCaptureReceived notification");

                                    if (OnNewCaptureReceived != null)
                                    {
                                        await
                                            OnNewCaptureReceived
                                                .Invoke();
                                    }
                                }
                            )
                    );

                _hubSubscriptions
                    .Add(
                        _hubConnection
                            .On(
                                "NewExecutionUpdateReceived",
                                async () =>
                                {
                                    logger
                                        .LogInformation("Received NewExecutionUpdateReceived notification");

                                    if (OnNewExecutionUpdateReceived != null)
                                    {
                                        await
                                            OnNewExecutionUpdateReceived
                                                .Invoke();
                                    }
                                }
                            )
                    );

                _hubSubscriptions
                    .Add(
                        _hubConnection
                            .On<bool>(
                                "NewConnectorUpdateReceived",
                                async success =>
                                {
                                    logger
                                        .LogInformation(
                                            "Received NewConnectorUpdateReceived notification with success={success}",
                                            success
                                        );

                                    if (OnNewConnectorUpdateReceived != null)
                                    {
                                        await
                                            OnNewConnectorUpdateReceived
                                                .Invoke(success);
                                    }
                                }
                            )
                    );

                _hubConnection.Reconnecting += error =>
                {
                    logger
                        .LogWarning(
                            "SignalR reconnecting: {error}",
                            error?.Message
                        );

                    return
                        Task
                            .CompletedTask;
                };

                _hubConnection.Reconnected += connectionId =>
                {
                    logger
                        .LogInformation(
                            "SignalR reconnected: {connectionId}",
                            connectionId
                        );

                    return
                        Task
                            .CompletedTask;
                };

                _hubConnection.Closed += error =>
                {
                    logger
                        .LogError(
                            "SignalR connection closed: {error}",
                            error?.Message
                        );

                    return
                        Task
                            .CompletedTask;
                };

                await
                    _hubConnection
                        .StartAsync();

                logger
                    .LogInformation("Connected to CaptureHub");
            }
            catch (TaskCanceledException ex)
            {
                logger
                    .LogWarning("CaptureHub start task cancelled");
            }
        }

        public async Task StopAsync()
        {
            foreach (var sub in _hubSubscriptions) 
                sub
                    .Dispose();
            
            _hubSubscriptions
                .Clear();
            
            if (_hubConnection != null)
            {
                if (_hubConnection.State != HubConnectionState.Disconnected)
                {
                    await
                        _hubConnection
                            .StopAsync();
                }

                await
                    _hubConnection
                        .DisposeAsync();

                _hubConnection = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await
                StopAsync();
        }

        private class AuthHeaderHandler(string authHeader) : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken
            )
            {
                if (!string.IsNullOrEmpty(authHeader))
                {
                    request.Headers.Authorization =
                        AuthenticationHeaderValue
                            .Parse(authHeader);
                }

                return
                    base
                        .SendAsync(
                            request,
                            cancellationToken
                        );
            }
        }
    }
}