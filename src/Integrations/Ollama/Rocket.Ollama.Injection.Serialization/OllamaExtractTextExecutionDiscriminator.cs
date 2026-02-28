using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts.Text;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaExtractTextExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OllamaExtractTextExecutionStepSpecifics);
        
        public string Value => "ollama_extract_text_execution";
    }
}