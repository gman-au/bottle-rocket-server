using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.GlobalSettings;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestGlobalSettings
    {
        Task<GlobalSettingsSpecifics> GetGlobalSettingsAsync(
            CancellationToken cancellationToken
        );

        Task<ApiResponse> UpdateGlobalSettingsAsync(
            GlobalSettingsSpecifics globalSettings,
            CancellationToken cancellationToken
        );
    }
}