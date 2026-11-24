namespace Rocket.Domain.Transcriptions
{
    public record TextTranscription : BaseTranscription
    {
        public string Text { get; set; }
    }
}