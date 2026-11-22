using System;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts;

namespace Rocket.OpenAi.Injection.Serialization
{
    public class OpenAiConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(OpenAiConnectorSpecifics);
        
        public string Value => "openai_connector";
    }
}