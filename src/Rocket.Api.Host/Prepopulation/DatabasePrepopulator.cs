using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Prepopulation
{
    public class DatabasePrepopulator(
        IRocketbookPageTemplateRepository rocketbookPageTemplateRepository,
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
    }
}