using Rocket.Domain.Enum;
using Rocket.Domain.Workflows;

namespace Rocket.Domain.Vendors.Temporary
{
    public record EmailFileAttachmentWorkflowStep : BaseWorkflowStep
    {
        public override int InputType { get; set; } = (int)WorkflowFormatTypeEnum.File;

        public override int OutputType { get; set; } = (int)WorkflowFormatTypeEnum.Void;

        public override string StepName { get; set; } = "Send file attachment as email";

        // metadata here e.g. "SubFolder", "FileName"

        public string TargetEmailAddress { get; set; }
    }
}