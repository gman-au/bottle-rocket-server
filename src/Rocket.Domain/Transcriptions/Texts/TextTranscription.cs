namespace Rocket.Domain.Transcriptions.Texts
{
    public record TextTranscription : BaseTranscription
    {
        public string Text { get; set; }
    }
}