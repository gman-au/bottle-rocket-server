using Rocket.Domain.Executions;

namespace Rocket.Google.Domain
{
    public record GoogleDriveUploadExecutionStep : BaseExecutionStep
    {
        public string ParentFolderId { get; set; }
    }
}