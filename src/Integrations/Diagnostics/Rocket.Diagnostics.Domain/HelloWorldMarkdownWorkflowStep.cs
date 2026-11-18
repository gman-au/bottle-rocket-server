using Rocket.Domain.Workflows;

namespace Rocket.Diagnostics.Domain
{
    public record HelloWorldMarkdownWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = DiagnosticDomainConstants.HelloWorldMarkdownInputTypes;

        public override int OutputType { get; set; } = DiagnosticDomainConstants.HelloWorldMarkdownOutputType;

        public override string StepName { get; set; } = "Hello world markdown (test workflow)";

        public override string StepCode { get; set; } = DiagnosticDomainConstants.HelloWorldMarkdownWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = string.Empty;

    }
}