using System;

namespace Rocket.Domain.ScannedImages
{
    public class Transcription
    {
        public string Id { get; set; }
        
        public string ExecutionId { get; set; }
        
        public string ExecutionName { get; set; }
        
        public DateTime TranscriptionDate { get; set; }
        
        public string Text { get; set; }
    }
}