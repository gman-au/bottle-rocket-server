using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts.Project;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiExtractProjectWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OpenAiExtractProjectWorkflowStepSpecifics);
        
        public string Value => "openai_extract_project_workflow";
    }
}