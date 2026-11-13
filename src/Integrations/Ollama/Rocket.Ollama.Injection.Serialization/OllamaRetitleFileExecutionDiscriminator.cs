using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.Retitle;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaRetitleFileExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OllamaRetitleFileExecutionStepSpecifics);
        
        public string Value => "ollama_retitle_file_execution";
    }
}