using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.Text;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiExtractTextWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OpenAiExtractTextWorkflowStepSpecifics);
        
        public string Value => "openai_extract_text_workflow";
    }
}