using Rocket.Domain.Workflows;

namespace Rocket.Dropbox.Domain
{
    public record DropboxUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = DropboxDomainConstants.DropboxUploadInputTypes;

        public override int OutputType { get; set; } = DropboxDomainConstants.DropboxUploadOutputType;

        public override string StepName { get; set; } = "Upload file to Dropbox";
        
        public override string StepCode { get; set; } = DropboxDomainConstants.UploadWorkflowCode;

        public override string RequiresConnectorCode { get; set; } = DropboxDomainConstants.ConnectorCode;

        public string Subfolder { get; set; }
    }
}