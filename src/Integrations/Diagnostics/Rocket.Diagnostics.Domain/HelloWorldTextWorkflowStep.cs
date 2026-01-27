using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Diagnostics.Domain
{
    public record HelloWorldTextWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.RawTextData;

        public override string StepName { get; set; } = "Hello world text (test workflow)";

        public override string RequiresConnectorCode { get; set; } = string.Empty;

    }
}