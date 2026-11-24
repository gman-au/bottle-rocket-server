using System;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Json.Transcriptions
{
    public class ProjectTranscriptionDiscriminator : IJsonTypeDiscriminator<BaseTranscription>
    {
        public Type Key => typeof(ProjectTranscriptionSummary);
        
        public string Value => "project_transcription";
    }
}