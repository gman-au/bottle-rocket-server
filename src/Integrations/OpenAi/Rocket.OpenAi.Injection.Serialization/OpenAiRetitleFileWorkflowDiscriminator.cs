using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.Retitle;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiRetitleFileWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OpenAiRetitleFileWorkflowStepSpecifics);
        
        public string Value => "openai_retitle_file_workflow";
    }
}