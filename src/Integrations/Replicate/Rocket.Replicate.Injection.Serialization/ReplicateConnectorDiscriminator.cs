using System;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts;

namespace Rocket.Replicate.Injection.Serialization
{
    public class ReplicateConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(ReplicateConnectorSpecifics);
        
        public string Value => "replicate_connector";
    }
}