using Rocket.Domain.Enum;
using Rocket.Domain.Utils;
using Rocket.Domain.Workflows;

namespace Rocket.Domain.Vendors.Dropbox
{
    public record DropboxUploadWorkflowStep : BaseWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.File, (int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload file to Dropbox";

        public override string RequiresConnectorCode { get; set; } = DomainConstants.ConnectorCodeDropboxApi;

        public string Subfolder { get; set; }
    }
}