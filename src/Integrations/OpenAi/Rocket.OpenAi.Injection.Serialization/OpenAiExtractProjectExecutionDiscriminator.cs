using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.Project;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiExtractProjectExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OpenAiExtractProjectExecutionStepSpecifics);
        
        public string Value => "openai_extract_project_execution";
    }
}