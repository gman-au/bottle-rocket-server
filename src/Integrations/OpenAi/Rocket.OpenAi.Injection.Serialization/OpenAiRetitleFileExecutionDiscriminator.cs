using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.Retitle;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiRetitleFileExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OpenAiRetitleFileExecutionStepSpecifics);
        
        public string Value => "openai_retitle_file_execution";
    }
}