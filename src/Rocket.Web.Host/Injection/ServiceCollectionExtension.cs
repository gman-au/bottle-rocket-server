using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Rocket.Interfaces;
using Rocket.Web.Host.Api;
using Rocket.Web.Host.Authentication;
using Rocket.Web.Host.HubClients;
using Rocket.Web.Host.Infrastructure;
using Rocket.Web.Host.Options;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace Rocket.Web.Host.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSignalRClientServices(this IServiceCollection services)
        {
            services
                .AddScoped<ICaptureHubClient, CaptureHubClient>();
            
            return services;
        }

        public static IServiceCollection AddBottleRocketWebServices(
            this IServiceCollection services,
            ConfigurationManager configuration,
            IWebHostEnvironment environment
        )
        {
            services
                .Configure<ApiConfigurationOptions>(
                    configuration
                        .GetSection(nameof(ApiConfigurationOptions))
                );
            
            services
                .Configure<SiteConfigurationOptions>(
                    configuration
                        .GetSection(nameof(SiteConfigurationOptions))
                );

            services
                .AddTransient<IApiRequestManager, ApiRequestManager>()
                .AddTransient<IWebHostErrorHandler, WebHostErrorHandler>();

            return services;
        }

        public static IServiceCollection AddMudBlazorServices(
            this IServiceCollection services,
            IConfigurationRoot configuration
        )
        {
            services
                .AddMudServices(
                    config =>
                    {
                        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                        config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
                        config.SnackbarConfiguration.BackgroundBlurred = true;
                        config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
                        config.SnackbarConfiguration.PreventDuplicates = false;
                        config.SnackbarConfiguration.VisibleStateDuration = 5000;
                        config.SnackbarConfiguration.HideTransitionDuration = 500;
                        config.SnackbarConfiguration.ShowTransitionDuration = 500;
                    }
                );

            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services
                .AddScoped<IAuthenticationManager, BasicAuthenticationManager>()
                .AddScoped<IAuthenticatedApiClient, AuthenticatedApiClient>();

            services
                .AddScoped<ApiAuthenticationStateProvider>();

            services
                .AddScoped<AuthenticationStateProvider>(
                    sp =>
                        sp.GetRequiredService<ApiAuthenticationStateProvider>()
                );

            return services;
        }
    }
}