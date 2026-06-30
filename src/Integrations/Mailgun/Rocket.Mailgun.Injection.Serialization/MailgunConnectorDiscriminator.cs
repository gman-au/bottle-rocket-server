using System;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;
using Rocket.Mailgun.Contracts;

namespace Rocket.Mailgun.Injection.Serialization
{
    public class MailgunConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(MailgunConnectorSpecifics);
        
        public string Value => "mailgun_connector";
    }
}