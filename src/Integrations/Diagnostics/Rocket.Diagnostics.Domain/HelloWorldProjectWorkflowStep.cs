using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Diagnostics.Domain
{
    public record HelloWorldProjectWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData;

        public override string StepName { get; set; } = "Hello world project (test workflow)";

        public override string StepCode { get; set; } = DiagnosticDomainConstants.HelloWorldProjectWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = string.Empty;

    }
}