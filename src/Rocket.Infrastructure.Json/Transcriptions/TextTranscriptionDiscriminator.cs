using System;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Json.Transcriptions
{
    public class TextTranscriptionDiscriminator : IJsonTypeDiscriminator<BaseTranscription>
    {
        public Type Key => typeof(TextTranscriptionSummary);
        
        public string Value => "text_transcription";
    }
}