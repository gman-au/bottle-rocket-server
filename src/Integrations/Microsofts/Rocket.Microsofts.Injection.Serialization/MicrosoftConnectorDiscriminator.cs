using System;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;

namespace Rocket.Microsofts.Injection.Serialization
{
    public class MicrosoftConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(MicrosoftConnectorSpecifics);
        
        public string Value => "microsoft_connector";
    }
}