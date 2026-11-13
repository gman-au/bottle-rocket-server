namespace Rocket.Dropbox.Domain
{
    public class DropboxFile
    {
        public byte[] Artifact { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string ScanId { get; set; }

        public string ExecutionStepId { get; set; }
    }
}