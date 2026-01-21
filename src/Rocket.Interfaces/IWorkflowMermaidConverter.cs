using Rocket.Api.Contracts.Workflows;

namespace Rocket.Interfaces
{
    public interface IWorkflowMermaidConverter
    {
        string Convert(WorkflowSummary workflow);
    }
}