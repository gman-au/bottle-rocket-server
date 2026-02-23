using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Injection.Web
{
    public class MicrosoftCustomConnectorProduct : ISkuConnector
    {
        public string Name => $"{MicrosoftDomainConstants.ConnectorName} (Custom)";
        public string Href => "/MyConnector/Microsoft/Add";
        public string ImagePath => "/img/microsoft-logo.png";
    }
}