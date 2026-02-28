using System;
using Rocket.Domain.Connectors;
using Rocket.Gcp.Contracts;
using Rocket.Interfaces;

namespace Rocket.Gcp.Injection.Serialization
{
    public class GcpConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(GcpConnectorSpecifics);
        
        public string Value => "gcp_connector";
    }
}