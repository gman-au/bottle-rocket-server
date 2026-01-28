using Rocket.Domain.Executions;

namespace Rocket.Microsoft.Domain
{
    public record OneDriveUploadExecutionStep : BaseExecutionStep
    {
        public string Subfolder { get; set; }
    }
}