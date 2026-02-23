using Rocket.Google.Domain;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Web
{
    public class GoogleConnectorProduct : ISkuConnector
    {
        public string Name => GoogleDomainConstants.ConnectorName;
        public string Href => "/MyConnector/Google/Add";
        public string ImagePath => "/img/google-logo.webp";
    }
}