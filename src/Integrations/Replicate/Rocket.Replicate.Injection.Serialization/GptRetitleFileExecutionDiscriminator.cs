using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.Gpt;

namespace Rocket.Replicate.Injection.Serialization
{
    public class GptRetitleFileExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(GptRetitleFileExecutionStepSpecifics);
        
        public string Value => "replicate_gpt_retitle_file_execution";
    }
}