using System;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;
using Rocket.Ollama.Contracts;

namespace Rocket.Ollama.Injection.Serialization
{
    public class OllamaConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(OllamaConnectorSpecifics);
        
        public string Value => "ollama_connector";
    }
}