using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Injection.Web
{
    public class ReplicateConnectorProduct : ISkuConnector
    {
        public string Name => ReplicateDomainConstants.ConnectorName;
        public string Href => "/MyConnector/Replicate/Add";
        public string ImagePath => "/img/replicate-logo.png";
    }
}