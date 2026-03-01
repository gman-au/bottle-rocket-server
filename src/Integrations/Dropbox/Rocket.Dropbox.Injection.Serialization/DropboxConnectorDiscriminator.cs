using System;
using Rocket.Domain.Connectors;
using Rocket.Dropbox.Contracts;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection.Serialization
{
    public class DropboxConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(DropboxConnectorSpecifics);
        
        public string Value => "dropbox_connector";
    }
}