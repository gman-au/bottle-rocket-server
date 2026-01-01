using System;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Rocket.Interfaces;
using Rocket.Web.Host.Authentication;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace Rocket.Web.Host.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBottleRocketWebServices(
            this IServiceCollection services,
            ConfigurationManager configuration,
            IWebHostEnvironment environment
        )
        {
            var apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new ConfigurationErrorsException();
            
            services
                .AddHttpClient<IAuthenticationManager, BasicAuthenticationManager>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });
            
            services
                .AddHttpClient<IAuthenticatedApiClient, AuthenticatedApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });
            
            services
                .AddTransient<IAuthenticationManager, BasicAuthenticationManager>()
                .AddTransient<IAuthenticatedApiClient, AuthenticatedApiClient>();
            
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
    }
}