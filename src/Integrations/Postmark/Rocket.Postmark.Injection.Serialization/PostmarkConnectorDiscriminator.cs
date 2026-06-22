using System;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;
using Rocket.Postmark.Contracts;

namespace Rocket.Postmark.Injection.Serialization
{
    public class PostmarkConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(PostmarkConnectorSpecifics);
        
        public string Value => "postmark_connector";
    }
}