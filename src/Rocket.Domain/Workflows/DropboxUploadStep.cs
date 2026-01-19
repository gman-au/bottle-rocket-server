using Rocket.Domain.Enum;

namespace Rocket.Domain.Workflows
{
    public record DropboxUploadStep : BaseWorkflowStep
    {
        public override int InputType { get; set; } = (int)WorkflowFormatTypeEnum.File;

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Upload file to Dropbox";

        public string Subfolder { get; set; }
    }
}