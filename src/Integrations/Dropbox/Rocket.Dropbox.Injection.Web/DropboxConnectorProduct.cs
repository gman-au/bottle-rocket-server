using Rocket.Dropbox.Domain;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection.Web
{
    public class DropboxConnectorProduct : ISkuConnector
    {
        public string Name => DropboxDomainConstants.ConnectorName;
        public string Href => "/MyConnector/Dropbox/Add";
        public string ImagePath => "/img/dropbox-logo.webp";
    }
}