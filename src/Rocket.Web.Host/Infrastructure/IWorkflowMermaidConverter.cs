using Rocket.Api.Contracts.Workflows;

namespace Rocket.Web.Host.Infrastructure
{
    public interface IWorkflowMermaidConverter
    {
        string Convert(MyWorkflowSummary workflow);
    }
}