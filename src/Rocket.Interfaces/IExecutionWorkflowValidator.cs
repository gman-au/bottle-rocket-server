using System.Collections.Generic;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IExecutionWorkflowValidator
    {
        IEnumerable<string> GetMissingConnectors(Workflow workflow);
    }
}