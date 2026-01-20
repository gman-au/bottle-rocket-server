namespace Rocket.Domain.Jobs
{
    public class ExecutionStepArtifact
    {
        public int Result { get; set; }

        public int ArtifactDataFormat { get; set; }

        public byte[] Artifact { get; set; }

        public string FileExtension { get; set; }
    }
}