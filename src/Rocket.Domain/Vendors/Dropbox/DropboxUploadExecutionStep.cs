using Rocket.Domain.Executions;

namespace Rocket.Domain.Vendors.Dropbox
{
    public record DropboxUploadExecutionStep : BaseExecutionStep
    {
        public string Subfolder { get; set; }
    }
}