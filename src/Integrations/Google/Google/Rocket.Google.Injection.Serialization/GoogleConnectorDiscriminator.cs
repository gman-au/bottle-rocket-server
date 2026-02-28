using System;
using Rocket.Domain.Connectors;
using Rocket.Google.Contracts;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Serialization
{
    public class GoogleConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(GoogleConnectorSpecifics);
        
        public string Value => "google_connector";
    }
}