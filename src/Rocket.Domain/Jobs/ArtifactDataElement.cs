using System;

namespace Rocket.Domain.Jobs
{
    public record ArtifactDataElement
    {
        private string _fileName = Guid.NewGuid().ToString();
        
        public byte[] Artifact { get; set; }

        public string FileExtension { get; set; }

        public string ScanId { get; set; }

        public string FileName
        {
            get => _fileName;
            set => _fileName = string.IsNullOrWhiteSpace(value) ? Guid.NewGuid().ToString() : value;
        }
    }
}