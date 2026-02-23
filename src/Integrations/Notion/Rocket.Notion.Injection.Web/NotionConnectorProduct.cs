using Rocket.Interfaces;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Injection.Web
{
    public class NotionConnectorProduct : ISkuConnector
    {
        public string Name => NotionDomainConstants.ConnectorName;
        public string Href => "/MyConnector/Notion/Add";
        public string ImagePath => "/img/notion-logo.png";
    }
}