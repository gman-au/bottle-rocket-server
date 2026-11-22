using Rocket.Interfaces;
using Rocket.OpenAi.Domain;

namespace Rocket.OpenAi.Injection.Web
{
    public class OpenAiConnectorProduct : ISkuConnector
    {
        public string Name => OpenAiDomainConstants.ConnectorName;
        public string Href => "/MyConnector/OpenAi/Add";
        public string ImagePath => "/img/openai-logo.jpg";
    }
}