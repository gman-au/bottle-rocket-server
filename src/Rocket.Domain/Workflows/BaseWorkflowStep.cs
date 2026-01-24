using System.Collections.Generic;

namespace Rocket.Domain.Workflows
{
    public abstract record BaseWorkflowStep
    {
        public string Id { get; set; }

        public string ConnectorId { get; set; }

        public abstract int[] InputTypes { get; set; }

        public abstract int OutputType { get; set; }

        public abstract string StepName { get; set; }

        public abstract string RequiresConnectorCode { get; set; }

        public IEnumerable<BaseWorkflowStep> ChildSteps { get; set; }
    }
}