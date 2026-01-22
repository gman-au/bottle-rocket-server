using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.PageTemplates;

namespace Rocket.Interfaces
{
    public interface IPageTemplateRepository
    {
        Task<long> UpsertPageTemplateAsync(
            PageTemplate pageTemplate,
            CancellationToken cancellationToken
        );
    }
}