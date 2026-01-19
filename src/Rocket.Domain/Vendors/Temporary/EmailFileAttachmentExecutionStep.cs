using Rocket.Domain.Executions;

namespace Rocket.Domain.Vendors.Temporary
{
    public record EmailFileAttachmentExecutionStep : BaseExecutionStep
    {
        public string TargetEmailAddress { get; set; }
    }
}