using Rocket.Domain.Workflows;

namespace Rocket.Microsofts.Domain
{
    public record OneDriveUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = MicrosoftDomainConstants.OneDriveUploadInputTypes;

        public override int OutputType { get; set; } = MicrosoftDomainConstants.OneDriveUploadOutputType;

        public override string StepName { get; set; } = "Upload note to OneDrive";

        public override string StepCode { get; set; } = MicrosoftDomainConstants.OneDriveUploadWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = MicrosoftDomainConstants.ConnectorCode;

        public string Subfolder { get; set; }
    }
}