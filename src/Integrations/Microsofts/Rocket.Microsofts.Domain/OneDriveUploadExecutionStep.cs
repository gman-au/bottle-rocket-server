using Rocket.Domain.Executions;

namespace Rocket.Microsofts.Domain
{
    public record OneDriveUploadExecutionStep : BaseExecutionStep
    {
        public string Subfolder { get; set; }
    }
}