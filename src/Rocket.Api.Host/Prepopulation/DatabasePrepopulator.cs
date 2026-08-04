using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Prepopulation
{
    public class DatabasePrepopulator(
        IGlobalSettingsRepository globalSettingsRepository,
        ILogger<DatabasePrepopulator> logger
    ) : IDatabasePrepopulator
    {
        public async Task PopulateGlobalSettingsAsync(CancellationToken cancellationToken)
        {
            logger
                .LogInformation("Running global settings prepopulation...");

            await
                globalSettingsRepository
                    .UpdateGlobalSettingsAsync(cancellationToken);

            logger
                .LogInformation("Completed global settings prepopulation");
        }
    }
}