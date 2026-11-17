using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.Retitle;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaRetitleFileWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OllamaRetitleFileWorkflowStepSpecifics);
        
        public string Value => "ollama_retitle_file_workflow";
    }
}