using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Google.Domain
{
    public record GoogleDriveUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload note to Google Drive";

        public override string RequiresConnectorCode { get; set; } = GoogleDomainConstants.ConnectorCode;

        public string ParentFolderId { get; set; }
    }
}