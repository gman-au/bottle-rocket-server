using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.Text;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaExtractTextWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OllamaExtractTextWorkflowStepSpecifics);
        
        public string Value => "ollama_extract_text_workflow";
    }
}