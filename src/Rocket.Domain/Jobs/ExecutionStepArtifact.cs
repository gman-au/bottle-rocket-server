using System;
using Rocket.Domain.Enum;

namespace Rocket.Domain.Jobs
{
    public class ExecutionStepArtifact
    {
        private string _fileName = Guid.NewGuid().ToString();
        
        public static readonly ExecutionStepArtifact Empty = new()
        {
            Result = (int)ExecutionStatusEnum.Completed,
            ArtifactDataFormat = (int)WorkflowFormatTypeEnum.Void,
            Artifact = [],
            FileExtension = null
        };

        public int Result { get; set; }

        public int ArtifactDataFormat { get; set; }

        public byte[] Artifact { get; set; }

        public string FileExtension { get; set; }

        public string FileName
        {
            get => _fileName;
            set => _fileName = string.IsNullOrWhiteSpace(value) ? Guid.NewGuid().ToString() : value;
        }
    }
}