using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.PageTemplates;

namespace Rocket.Interfaces
{
    public interface IRocketbookPageTemplateRepository
    {
        Task<long> UpsertPageTemplateAsync(
            RocketbookPageTemplate rocketbookPageTemplate,
            CancellationToken cancellationToken
        );

        Task<IEnumerable<RocketbookPageTemplate>> FetchAllAsync(CancellationToken cancellationToken);
        
        Task<RocketbookPageTemplate> GetTemplateByQrCodeAsync(string qrCode, CancellationToken cancellationToken);
    }
}