using System.Collections.Generic;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ExecutionWorkflowValidator : IExecutionWorkflowValidator
    {
        public IEnumerable<string> GetMissingConnectors(Workflow workflow)
        {
            var missingConnectors = new List<string>();

            foreach (var childStep in workflow?.Steps ?? [])
                CheckConnectors(childStep, ref missingConnectors);

            return missingConnectors;
        }

        private static void CheckConnectors(
            BaseWorkflowStep step,
            ref List<string> missingConnectors
        )
        {
            if (string.IsNullOrEmpty(step.ConnectorId))
            {
                missingConnectors
                    .Add(
                        step.StepName
                    );
            }

            foreach (var childStep in step.ChildSteps ?? [])
                CheckConnectors(childStep, ref missingConnectors);
        }
    }
}