using Rocket.Domain.Workflows;

namespace Rocket.Google.Domain
{
    public record GoogleDriveUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = GoogleDomainConstants.GoogleDriveUploadInputTypes;

        public override int OutputType { get; set; } = GoogleDomainConstants.GoogleDriveUploadOutputType;

        public override string StepName { get; set; } = "Upload note to Google Drive";

        public override string StepCode { get; set; } = GoogleDomainConstants.UploadFileWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = GoogleDomainConstants.ConnectorCode;

        public string ParentFolderId { get; set; }
    }
}