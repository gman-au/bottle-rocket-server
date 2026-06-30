using Rocket.Interfaces;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Injection.Web
{
    public class MailgunConnectorProduct : ISkuConnector
    {
        public string Name => MailgunDomainConstants.ConnectorName;
        public string Href => "/MyConnector/Mailgun/Add";
        public string ImagePath => "/img/mailgun-logo.png";
    }
}