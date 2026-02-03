using Rocket.Domain.Executions;

namespace Rocket.Gcp.Domain
{
    public record GcpUploadExecutionStep : BaseExecutionStep
    {
        public string ParentFolderId { get; set; }
    }
}