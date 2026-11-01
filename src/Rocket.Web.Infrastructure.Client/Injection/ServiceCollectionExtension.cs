using Microsoft.Extensions.DependencyInjection;
using Rocket.Web.Infrastructure.Client.Vendors;

namespace Rocket.Web.Infrastructure.Client.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBottleRocketWebApiClient(
            this IServiceCollection services
        )
        {
            services
                .AddTransient<IApiRequestConnectors, ApiRequestConnectors>()
                .AddTransient<IApiRequestDashboard, ApiRequestDashboard>()
                .AddTransient<IApiRequestExecutions, ApiRequestExecutions>()
                .AddTransient<IApiRequestGlobalSettings, ApiRequestGlobalSettings>()
                .AddTransient<IApiRequestScans, ApiRequestScans>()
                .AddTransient<IApiRequestSessions, ApiRequestSessions>()
                .AddTransient<IApiRequestSystem, ApiRequestSystem>()
                .AddTransient<IApiRequestUsers, ApiRequestUsers>()
                .AddTransient<IApiRequestWorkflows, ApiRequestWorkflows>()
                .AddTransient<IApiRequestWorkflowSteps, ApiRequestWorkflowSteps>();

            services
                .AddTransient<IApiRequestDropbox, ApiRequestDropbox>()
                .AddTransient<IApiRequestGoogle, ApiRequestGoogle>()
                .AddTransient<IApiRequestMicrosoft, ApiRequestMicrosoft>()
                .AddTransient<IApiRequestNotion, ApiRequestNotion>();

            services
                .AddTransient<IApiRequestManager>(
                    sp =>
                        new ApiRequestManager(
                            sp.GetRequiredService<IApiRequestConnectors>(),
                            sp.GetRequiredService<IApiRequestDashboard>(),
                            sp.GetRequiredService<IApiRequestExecutions>(),
                            sp.GetRequiredService<IApiRequestGlobalSettings>(),
                            sp.GetRequiredService<IApiRequestScans>(),
                            sp.GetRequiredService<IApiRequestSessions>(),
                            sp.GetRequiredService<IApiRequestSystem>(),
                            sp.GetRequiredService<IApiRequestUsers>(),
                            sp.GetRequiredService<IApiRequestVendors>(),
                            sp.GetRequiredService<IApiRequestWorkflows>(),
                            sp.GetRequiredService<IApiRequestWorkflowSteps>()
                        )
                );

            services
                .AddTransient<IApiRequestVendors>(
                    sp =>
                        new ApiRequestVendors(
                            sp.GetRequiredService<IApiRequestDropbox>(),
                            sp.GetRequiredService<IApiRequestGoogle>(),
                            sp.GetRequiredService<IApiRequestMicrosoft>(),
                            sp.GetRequiredService<IApiRequestNotion>()
                        )
                );

            return services;
        }
    }
}