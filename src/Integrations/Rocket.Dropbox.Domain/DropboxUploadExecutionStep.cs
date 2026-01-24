using Rocket.Domain.Core;

namespace Rocket.Dropbox.Domain
{
    public record DropboxUploadExecutionStep : CoreExecutionStep
    {
        public string Subfolder { get; set; }
    }
}