using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Domain.Vendors.Dropbox
{
    public record DropboxUploadWorkflowStep : BaseWorkflowStep
    {
        public override int InputType { get; set; } = (int)WorkflowFormatTypeEnum.File;

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload file to Dropbox";

        public string Subfolder { get; set; }
    }
}