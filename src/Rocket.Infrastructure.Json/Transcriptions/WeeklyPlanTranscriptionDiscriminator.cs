using System;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Json.Transcriptions
{
    public class WeeklyPlanTranscriptionDiscriminator : IJsonTypeDiscriminator<BaseTranscription>
    {
        public Type Key => typeof(WeeklyPlanTranscriptionSummary);
        
        public string Value => "weekly_plan_transcription";
    }
}