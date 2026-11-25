using Rocket.Domain.Enum;

namespace Rocket.Domain.Transcriptions.Texts
{
    public record TextTranscription : BaseTranscription
    {
        public string Text { get; set; }
        
        public override int WorkflowFormatType => (int)WorkflowFormatTypeEnum.RawTextData;
    }
}