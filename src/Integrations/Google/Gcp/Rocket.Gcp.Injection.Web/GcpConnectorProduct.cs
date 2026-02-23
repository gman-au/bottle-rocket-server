using Rocket.Gcp.Domain;
using Rocket.Interfaces;

namespace Rocket.Gcp.Injection.Web
{
    public class GcpConnectorProduct : ISkuConnector
    {
        public string Name => GcpDomainConstants.ConnectorName;
        public string Href => "/MyConnector/GcpExtract/Add";
        public string ImagePath => "/img/gcp-logo.png";
    }
}