using Rocket.Domain.Enum;

namespace Rocket.Domain.Workflows
{
    public record EmailFileAttachmentStep : BaseWorkflowStep
    {
        public override int InputType => (int)WorkflowFormatTypeEnum.File;

        public override int OutputType => (int)WorkflowFormatTypeEnum.Void;

        public override string StepName => "Send file attachment as email";

        // metadata here e.g. "SubFolder", "FileName"

        public string TargetEmailAddress { get; set; }
    }
}