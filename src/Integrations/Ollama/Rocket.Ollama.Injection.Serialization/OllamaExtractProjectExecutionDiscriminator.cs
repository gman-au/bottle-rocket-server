using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.Project;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaExtractProjectExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OllamaExtractProjectExecutionStepSpecifics);
        
        public string Value => "ollama_extract_project_execution";
    }
}