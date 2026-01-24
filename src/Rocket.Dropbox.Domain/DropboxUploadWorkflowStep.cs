using Rocket.Domain.Core;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Core.Utils;

namespace Rocket.Dropbox.Domain
{
    public record DropboxUploadWorkflowStep : CoreWorkflowStep
    {
        public override int[] InputTypes { get; set; } = [(int)WorkflowFormatTypeEnum.File, (int)WorkflowFormatTypeEnum.ImageData];

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload file to Dropbox";

        public override string RequiresConnectorCode { get; set; } = DomainConstants.ConnectorCodeDropboxApi;

        public string Subfolder { get; set; }
    }
}