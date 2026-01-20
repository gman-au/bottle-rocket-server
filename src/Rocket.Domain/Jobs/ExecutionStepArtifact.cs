namespace Rocket.Domain.Jobs
{
    public class ExecutionStepArtifact
    {
        public int Result { get; set; }

        public string ArtifactKey { get; set; }

        public byte[] Artifact { get; set; }
    }
}