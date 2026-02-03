using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.QuestPdf.Domain
{
    public record ConvertToPdfWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.File;

        public override string StepName { get; set; } = "Convert data to PDF";

        public override string RequiresConnectorCode { get; set; } = string.Empty;
    }
}