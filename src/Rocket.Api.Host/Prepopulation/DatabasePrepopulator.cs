using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Prepopulation
{
    public class DatabasePrepopulator(
        IRocketbookPageTemplateRepository rocketbookPageTemplateRepository,
        IGlobalSettingsRepository globalSettingsRepository,
        ILogger<DatabasePrepopulator> logger
    ) : IDatabasePrepopulator
    {
        public async Task PopulatePageTemplatesAsync(CancellationToken cancellationToken)
        {
            logger
                .LogWarning("Running page template prepopulation...");

            var recordsUpdated = 0L;

            foreach (var pageTemplate in PageTemplates.GetRocketbookTemplates())
            {
                recordsUpdated +=
                    await
                        rocketbookPageTemplateRepository
                            .UpsertPageTemplateAsync(
                                pageTemplate,
                                cancellationToken
                            );
            }

            logger
                .LogInformation("Modified {code} page template records", recordsUpdated);
        }

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