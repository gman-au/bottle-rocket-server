using System;
using System.Collections.Generic;
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
            Elements = [],
            UserId = null,
            BatchId = null
        };
        
        public IEnumerable<ArtifactDataElement> Elements { get; set; }

        public int Result { get; set; }

        public int ArtifactDataFormat { get; set; }

        public string BatchId { get; set; }
        
        public string UserId { get; set; }

        public string FileName
        {
            get => _fileName;
            set => _fileName = string.IsNullOrWhiteSpace(value) ? Guid.NewGuid().ToString() : value;
        }
    }
}