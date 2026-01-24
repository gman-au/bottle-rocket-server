using System.Collections.Generic;

namespace Rocket.Domain.Core
{
    public abstract record CoreWorkflowStep
    {
        public string Id { get; set; }

        public string ConnectorId { get; set; }

        public abstract int[] InputTypes { get; set; }

        public abstract int OutputType { get; set; }

        public abstract string StepName { get; set; }

        public abstract string RequiresConnectorCode { get; set; }

        public IEnumerable<CoreWorkflowStep> ChildSteps { get; set; }
    }
}