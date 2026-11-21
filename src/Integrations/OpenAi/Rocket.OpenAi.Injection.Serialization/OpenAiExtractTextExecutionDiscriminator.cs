using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.Text;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiExtractTextExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OpenAiExtractTextExecutionStepSpecifics);
        
        public string Value => "openai_extract_text_execution";
    }
}