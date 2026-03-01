using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.Project;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaExtractProjectWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OllamaExtractProjectWorkflowStepSpecifics);
        
        public string Value => "ollama_extract_project_workflow";
    }
}