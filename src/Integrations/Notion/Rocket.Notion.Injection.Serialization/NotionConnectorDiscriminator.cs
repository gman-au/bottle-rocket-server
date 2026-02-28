using System;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;
using Rocket.Notion.Contracts;

namespace Rocket.Notion.Injection.Serialization
{
    public class NotionConnectorDiscriminator : IJsonTypeDiscriminator<BaseConnector>
    {
        public Type Key => typeof(NotionConnectorSpecifics);
        
        public string Value => "notion_connector";
    }
}