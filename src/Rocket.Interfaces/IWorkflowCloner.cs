using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IWorkflowCloner
    {
        Execution Clone(Workflow workflow);
    }
}