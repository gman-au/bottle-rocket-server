using Rocket.Domain.Workflows;

namespace Rocket.Diagnostics.Domain
{
    public record HelloWorldTextWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = DiagnosticDomainConstants.HelloWorldTextInputTypes;

        public override int OutputType { get; set; } = DiagnosticDomainConstants.HelloWorldTextOutputType;

        public override string StepName { get; set; } = "Hello world text (test workflow)";

        public override string StepCode { get; set; } = DiagnosticDomainConstants.HelloWorldTextWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = string.Empty;

    }
}