using Rocket.Domain.Enum;

namespace Rocket.Domain.Workflows
{
    public record EmailFileAttachmentStep : BaseWorkflowStep
    {
        public override int InputType { get; set; } = (int)WorkflowFormatTypeEnum.File;

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Send file attachment as email";

        // metadata here e.g. "SubFolder", "FileName"

        public string TargetEmailAddress { get; set; }
    }
}