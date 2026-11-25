using System.Threading.Tasks;

namespace Rocket.Web.Host.Infrastructure
{
    public interface ITemplateDownloader
    {
        bool HasATemplate(
            int workflowType
        );

        Task GetTemplateForWorkflowTypeAsync(
            int workflowType
        );
    }
}