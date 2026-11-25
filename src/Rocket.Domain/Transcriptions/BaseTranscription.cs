using System;

namespace Rocket.Domain.Transcriptions
{
    public abstract record BaseTranscription
    {
        public string Id { get; set; }
        
        public string ExecutionId { get; set; }
        
        public string ExecutionStepName { get; set; }
        
        public DateTime TranscriptionDate { get; set; }
        
        public abstract int WorkflowFormatType { get; }
    }
}