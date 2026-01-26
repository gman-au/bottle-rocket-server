using Rocket.Domain.Enum;

namespace Rocket.Domain.Jobs
{
    public class ExecutionStepArtifact
    {
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
    }
}