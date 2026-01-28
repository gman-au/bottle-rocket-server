using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Microsoft.Domain
{
    public record OneDriveUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload note to OneDrive";

        public override string RequiresConnectorCode { get; set; } = MicrosoftDomainConstants.ConnectorCode;

        public string Subfolder { get; set; }
    }
}