using Rocket.Interfaces;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Injection.Web
{
    public class MicrosoftPreMadeConnectorProduct : ISkuConnector
    {
        public string Name => $"{MicrosoftDomainConstants.ConnectorName} (Bottle-Rocket)";
        public string Href => "/MyConnector/Microsoft/Add/cb829057-cb6e-45c3-90a7-5fcf40edacae/common";
        public string ImagePath => "/img/microsoft-logo.png";
    }
}