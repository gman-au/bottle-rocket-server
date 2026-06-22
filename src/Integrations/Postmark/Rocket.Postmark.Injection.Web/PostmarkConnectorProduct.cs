using Rocket.Interfaces;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Injection.Web
{
    public class PostmarkConnectorProduct : ISkuConnector
    {
        public string Name => PostmarkDomainConstants.ConnectorName;
        public string Href => "/MyConnector/Postmark/Add";
        public string ImagePath => "/img/postmark-logo.jpg";
    }
}