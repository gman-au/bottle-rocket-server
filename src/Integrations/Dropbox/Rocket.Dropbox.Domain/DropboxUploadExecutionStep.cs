using Rocket.Domain.Executions;

namespace Rocket.Dropbox.Domain
{
    public record DropboxUploadExecutionStep : BaseExecutionStep
    {
        public string Subfolder { get; set; }
    }
}